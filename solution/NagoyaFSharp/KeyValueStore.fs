module KeyValueStore

let empty = []

let put k v kvs =
  (k, v) :: kvs

let toStr kvs =
  ""