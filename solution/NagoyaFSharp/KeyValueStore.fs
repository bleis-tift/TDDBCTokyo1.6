﻿module KeyValueStore

let empty = []

let init kvs = kvs

let private eq key (k, _) = k = key

let put k v kvs =
  (k, v) :: (kvs |> List.filter (eq k >> not))

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