#!/bin/bash

. ../util.sh

read_input_file $(dirname "$0")/input

prepare_input_data () {
    splitValues=$(echo "$input" | tr ',' '\n')
    
    echo "$splitValues" | while IFS='-' read -r startcode endcode; do
        echo "startcode=$startcode endcode=$endcode"
    done
}

process_product_codes () {
    local invalidCodesValues=0

    local temp_dir=$(mktemp -d)
    local thread_counter=0
    
    while IFS=' ' read -r startcode endcode; do
        (
            local thread_file="$temp_dir/thread_${thread_counter}.txt"
            local thread_invalid_sum=0
            
            for ((code=startcode; code<=endcode; code++)); do
                isValid=$(is_product_code_valid "$code")
                if [[ "$isValid" == "0" ]]; then
                    echo "Invalid code: $code" >&2
                    ((thread_invalid_sum = thread_invalid_sum + $code))
                fi
            done
            
            echo "$thread_invalid_sum" > "$thread_file"
        ) &
        ((thread_counter++))
    done < <(prepare_input_data)
    
    wait
    
    for thread_file in "$temp_dir"/thread_*.txt; do
        if [[ -f "$thread_file" ]]; then
            invalidCodesValues=$((invalidCodesValues + $(cat "$thread_file")))
        fi
    done
    
    rm -rf "$temp_dir"

    echo "$invalidCodesValues"
}

is_product_code_valid () {
    local code="$1"
    local len="${#code}"
    local isValid=1
    local half_len=$((len / 2))

    # Only go until half the length of the code, as larger segments can't repeat
    for ((i=1; i<=half_len; i++)); do
        local segment_length=$i
        local num_segments=$((len / segment_length))
        
        # Only check if the code can be evenly divided into segments
        if (( len % segment_length == 0 )); then
            local first_segment="${code:0:segment_length}"
            local all_same=1
            
            # Check all segments one by one
            for ((j=1; j<num_segments; j++)); do
                local current_segment="${code:j*segment_length:segment_length}"
                if [[ "$current_segment" != "$first_segment" ]]; then
                    all_same=0
                    break
                fi
            done
            
            if [[ $all_same -eq 1 ]]; then
                isValid=0
                echo "Found repeating segment '$first_segment' in code '$code'" >&2
                break
            fi
        fi
    done
    
    echo $isValid
}

echo "Starting product code scanner..."
invalidCodes=$(process_product_codes)
echo "Total count of invalid codes: $invalidCodes"
echo "Product code scanning complete."