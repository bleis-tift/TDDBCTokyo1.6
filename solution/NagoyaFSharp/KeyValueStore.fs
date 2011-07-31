module KeyValueStore

open System

let mutable dt: DateTime option = None

let now () = match dt with Some dt -> dt | None -> DateTime.Now

let empty = []

let init kvs = kvs

let private eq key (k, _) = k = key

let put k v kvs =
  (k, v) :: (kvs |> List.filter (eq k >> not))

let putAll xs kvs =
  xs |> List.fold (fun st (k, v) -> st |> put k v) kvs

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