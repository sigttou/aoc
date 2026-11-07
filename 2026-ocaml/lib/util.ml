(* Every day module must expose this shape so the runner can use it as-is. *)
module type S = sig
  val part1 : string -> string
  val part2 : string -> string
end

let read_file path = In_channel.with_open_bin path In_channel.input_all

let input_path day = Printf.sprintf "data/inputs/%02d.txt" day
let example_path day = Printf.sprintf "data/examples/%02d.txt" day

let int_list input =
  input
  |> String.split_on_char '\n'
  |> List.filter_map (fun line ->
         match String.trim line with "" -> None | l -> Some (int_of_string l))
