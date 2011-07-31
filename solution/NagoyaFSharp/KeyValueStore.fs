module KeyValueStore

let empty = []

let init kvs = kvs

let private eq key (k, _) = k = key

let put k v kvs =
  if kvs |> List.exists (eq k) then (k, v) :: (kvs |> List.filter (eq k >> not))
  else (k, v) :: kvs

let get key kvs =
  kvs
  |> List.tryFind (eq key)
  |> Option.map snd    

let delete key kvs =
  kvs |> List.filter(eq key >> not) 

let toStr kvs =
  sprintf "%A" kvs

let dump kvs =
  do printf "%s" (kvs |> toStr)