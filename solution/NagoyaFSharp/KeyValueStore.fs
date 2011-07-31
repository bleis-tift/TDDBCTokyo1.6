module KeyValueStore

let empty = []

let put k v kvs =
  (k, v) :: kvs

let get key kvs =
  kvs
  |> List.tryFind (fun (k, _) -> k = key)
  |> Option.map snd    

let toStr kvs =
  sprintf "%A" kvs

let dump kvs =
  do printf "%s" (kvs |> toStr)