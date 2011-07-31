module KeyValueStore

let empty = []

let init kvs = kvs

let put k v kvs =
  (k, v) :: kvs

let get key kvs =
  kvs
  |> List.tryFind (fst >> ((=)key))
  |> Option.map snd    

let delete key kvs =
  kvs |> List.filter (fst >> ((<>)key))

let toStr kvs =
  sprintf "%A" kvs

let dump kvs =
  do printf "%s" (kvs |> toStr)