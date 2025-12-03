. ../util.sh

read_input_file $(dirname "$0")/input

NUM_OF_BATTERIES=12

calculate_total_joltage () {
    local banks=("${input[@]}")
    local total_joltage=0
    local banks_count=${#banks[@]}

    echo "Total banks: $banks_count" >&2
    for (( i=0; i<$banks_count; i++ )); do
        total_joltage=$((total_joltage + $(get_highest_bank_joltage "${banks[$i]}")))
    done

    echo $total_joltage
}

get_highest_bank_joltage () {
    local bank=$1
    local batteries_gathered=""
    local current_index=-1

    for (( i=0; i<NUM_OF_BATTERIES; i++ )); do
        current_index=$(get_next_highest_battery_index "$bank" $current_index $i)
        batteries_gathered="${batteries_gathered}${bank:$current_index:1}"
    done

    echo $batteries_gathered
}

get_next_highest_battery_index () {
    local bank=$1
    local index=$2
    local batteries_already_gathered=$3
    local number_of_batteries_available=$((${#bank} - (NUM_OF_BATTERIES - $batteries_already_gathered)))
    local highest=0

    for (( i=index+1; i<$number_of_batteries_available + 1; i++ )); do
        local number=${bank:$i:1}
        if (( number > highest )); then
            highest=$number
            index=$i
        fi
    done

    echo $index
}

echo "Total joltage: $(calculate_total_joltage)"