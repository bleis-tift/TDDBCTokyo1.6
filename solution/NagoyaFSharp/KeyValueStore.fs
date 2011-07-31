module KeyValueStore

let empty = []

let put k v kvs =
  (k, v) :: kvs

let toStr kvs =
  sprintf "%A" kvs

let dump kvs =
  do printf "%s" (kvs |> toStr)