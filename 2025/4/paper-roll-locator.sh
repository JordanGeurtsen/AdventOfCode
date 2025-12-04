#!/bin/bash

. ../util.sh

read_input_file $(dirname "$0")/input

ROLL_AVAILABLE_THRESHOLD=4
ROLL="@"
EMPTY="."
TO_BE_REMOVED_ROLL="%"
REMOVED_ROLL="X"

declare -A grid_2d=()

process_input_data() {
    local row=0

    for line in "${input[@]}"; do
        local col=0
        echo "Processing line: $line" >&2
        for (( i=0; i<${#line}; i++ )); do
            grid_2d[$row,$col]=${line:i:1}
            ((col++))
        done
        ((row++))
    done
}

locate_available_rolls() {
    process_input_data
    echo $(process_rolls_in_grid)
}

process_rolls_in_grid() {
    local available_rolls=0
    local rows=${#input[@]}
    local cols=${#input[0]}
    # echo "Grid dimensions: ${rows}x${cols}" >&2

    # Mark rolls to be removed
    for (( r=0; r<rows; r++ )); do
        for (( c=0; c<cols; c++ )); do
            local char=${grid_2d[$r,$c]}
            if [[ $char == "$ROLL" || $char == "$TO_BE_REMOVED_ROLL" ]]; then
                local ajdecent_roll_count=0

                # Check adjacent cells
                for dr in -1 0 1; do
                    for dc in -1 0 1; do
                        if (( dr == 0 && dc == 0 )); then
                            continue
                        fi
                        local nr=$((r + dr))
                        local nc=$((c + dc))
                        if (( nr >= 0 && nr < rows && nc >= 0 && nc < cols )); then
                            local nchar=${grid_2d[$nr,$nc]}
                            if [[ $nchar == "$ROLL" || $nchar == "$TO_BE_REMOVED_ROLL" ]]; then
                                ((ajdecent_roll_count++))
                            fi
                        fi
                    done
                done

                if (( ajdecent_roll_count < ROLL_AVAILABLE_THRESHOLD )); then
                    ((available_rolls++))
                    grid_2d[$r,$c]=$TO_BE_REMOVED_ROLL
                fi
            fi
        done
    done

    # Remove marked rolls
    for (( r=0; r<rows; r++ )); do
        for (( c=0; c<cols; c++ )); do
            if [[ ${grid_2d[$r,$c]} == "$TO_BE_REMOVED_ROLL" ]]; then
                grid_2d[$r,$c]=$REMOVED_ROLL
            fi
        done
    done

    echo "Rolls found in this pass: $available_rolls" >&2

    # Recursively process again if any rolls were removed to check for new available rolls
    if (( available_rolls > 0 )); then
        available_rolls=$((available_rolls + $(process_rolls_in_grid)))
    fi

    echo $available_rolls
}

echo "Total available rolls: $(locate_available_rolls)"