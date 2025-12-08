read_input_file () {
    local file_path="$1"
    echo "Reading input from: $file_path" >&2
    if [[ ! -f "$file_path" ]]; then
        echo "Input file not found!"
        exit 1
    fi
    mapfile -t input < "$file_path"
}