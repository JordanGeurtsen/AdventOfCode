#!/bin/bash

file_path="input"
dial_start_position=50
zero_count=0

read_input_file () {
    if [[ ! -f "$file_path" ]]; then
        echo "Input file not found!"
        exit 1
    fi
    mapfile -t dial_codes < "$file_path"
}

move_dial () {
    direction=$1
    amount=$2
    start_pos=$3
    
    if [[ "$direction" == "L" ]]; then
        new_pos=$(( (start_pos - amount + 100) % 100 ))
    else
        new_pos=$(( (start_pos + amount) % 100 ))
    fi

    echo $new_pos
}

count_zeros_in_path () {
    start=$1
    direction=$2
    amount=$3
    
    local zeros=0
    
    if [[ "$direction" == "L" ]]; then
        for ((i=1; i<=amount; i++)); do
            pos=$(( (start - i + 100) % 100 ))
            if [[ $pos -eq 0 ]]; then
                ((zeros++))
            fi
        done
    else
        for ((i=1; i<=amount; i++)); do
            pos=$(( (start + i) % 100 ))
            if [[ $pos -eq 0 ]]; then
                ((zeros++))
            fi
        done
    fi
    
    echo "$zeros zeroes in path!" >&2
    echo $zeros
}

calculate_final_dial_code () {
    current_position=$dial_start_position

    echo "Cracking the dial code..."

    for code in "${dial_codes[@]}"; do
        direction=${code:0:1}
        amount=${code:1}
        zeros_in_path=$(count_zeros_in_path "$current_position" "$direction" "$amount")
        zero_count=$(($zero_count + $zeros_in_path))
        current_position=$(move_dial "$direction" "$amount" "$current_position")
    done

    echo "Number of times dial landed on 0: $zero_count"
}

read_input_file
calculate_final_dial_code