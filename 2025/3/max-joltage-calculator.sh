. ../util.sh

read_input_file $(dirname "$0")/input

calculate_total_joltage () {
    local banks=("${input[@]}")
    local total_joltage=0
    local banks_count=${#banks[@]}

    echo "Total banks: $banks_count" >&2
    for (( i=0; i<$banks_count; i++ )); do
        total_joltage=$((total_joltage + $(calculate_max_bank_joltage "${banks[$i]}")))
    done

    echo $total_joltage
}

calculate_max_bank_joltage () {
    local bank=$1
    local first_battery_index=$(get_highest_first_battery_index "$bank")
    local second_battery_index=$(get_highest_second_battery_index "$bank" $first_battery_index)

    local first_digit=${bank:$first_battery_index:1}
    local second_digit=${bank:$second_battery_index:1}
    local max_joltage="${first_digit}${second_digit}"

    echo $max_joltage
}

get_highest_first_battery_index () {
    local bank=$1
    local highest=0
    local index=0

    for (( i=0; i<"${#bank}"-1; i++ )); do
        local number=${bank:$i:1}
        if (( number > highest )); then
            highest=$number
            index=$i
        fi
    done

    echo $index
}

get_highest_second_battery_index () {
    local bank=$1
    local first_battery_index=$2
    local highest=0
    local index=0

    for (( i=first_battery_index+1; i<"${#bank}"; i++ )); do
        local number=${bank:$i:1}
        if (( number > highest )); then
            highest=$number
            index=$i
        fi
    done

    echo $index
}

echo "Total joltage: $(calculate_total_joltage)"
