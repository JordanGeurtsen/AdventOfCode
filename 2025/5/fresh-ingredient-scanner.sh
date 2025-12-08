#!/bin/bash

. ../util.sh

read_input_file $(dirname "$0")/input

prepare_fresh_value_ranges() {
    # Save all lines until the first empty line
    local i
    for i in "${!input[@]}"; do
        if [[ -z "${input[$i]}" ]]; then
            echo "${input[@]:0:$i}"
            return
        fi
    done
    echo "${input[@]}"
}

prepare_ingredient_values() {
    # Loop until the first empty line, after that, return the rest of the array
    local i
    for i in "${!input[@]}"; do
        if [[ -z "${input[$i]}" ]]; then
            echo "${input[@]:$((i+1))}"
            return
        fi
    done
    echo ""
}

count_available_fresh_ingredients() {
    local -a fresh_ranges=($(prepare_fresh_value_ranges))
    local -a ingredients=($(prepare_ingredient_values))

    local count=0
    local ingredient
    local temp_file=$(mktemp)
    
    for ingredient in "${ingredients[@]}"; do
        {
            if [[ $(is_ingredient_fresh "$ingredient") -eq 1 ]]; then
                echo "1" >> "$temp_file"
            fi
        } &
    done
    
    wait
    count=$(wc -l < "$temp_file" 2>/dev/null || echo 0)
    rm -f "$temp_file"

    echo "$count"
}

is_ingredient_fresh() {
    local ingredient_value=$1
    local -a fresh_ranges=($(prepare_fresh_value_ranges))

    local range
    for range in "${fresh_ranges[@]}"; do
        IFS='-' read -r min max <<< "$range"
        if (( ingredient_value >= min && ingredient_value <= max )); then
            echo 1
            return
        fi
    done
    echo 0
}

count_fresh_ingredients() {
    echo "Counting total possible fresh ingredient values..." >&2
    local -a fresh_ranges=($(prepare_fresh_value_ranges))
    local -a fresh_ids=()

    local count=0
    local range
    
    # Loop through each range and count unique fresh IDs
    local temp_file=$(mktemp)
    
    local -a non_overlapping_ranges=()
    local -a overlapping_values=()
    
    echo "Identifying overlapping and non-overlapping ranges..." >&2

    for range in "${fresh_ranges[@]}"; do
        {
            IFS='-' read -r min max <<< "$range"
            local has_overlap=0
            echo "Checking range: $range" >&2

            # Check if this range overlaps with any other range
            for other_range in "${fresh_ranges[@]}"; do
                if [[ "$range" == "$other_range" ]]; then
                    continue
                fi
                IFS='-' read -r other_min other_max <<< "$other_range"
                
                # Check for overlap: ranges overlap if one starts before the other ends
                if (( min <= other_max && max >= other_min )); then
                    has_overlap=1

                    # Add all values from this range to overlapping list for processing
                    echo "Adding overlapping range: $range" >&2
                        for ((i = min; i <= max; i++)); do
                            echo "$i" >> "$temp_file"
                        done
                    break
                fi
            done
            
            if [[ $has_overlap -eq 0 ]]; then
                # No overlap - add count directly
                echo "Adding non-overlapping range: $range" >&2
                echo $((max - min + 1)) >> "$count_temp"
            fi
        }
    done
    
    echo "Processing overlapping ranges..." >&2
    
    # Process overlapping values - count only unique ones
    if [[ -f "$temp_file" && -s "$temp_file" ]]; then
        local overlap_count=$(sort -u "$temp_file" | wc -l)
        count=$((count + overlap_count))
    fi
    
    rm -f "$temp_file"
    echo "$count"
}

echo "Processing fresh ingredient data..."
echo "There are $(count_available_fresh_ingredients) fresh ingredients available."

echo "Checking all possible fresh ingredient values..."
echo "There are $(count_fresh_ingredients) total possible fresh ingredient values."