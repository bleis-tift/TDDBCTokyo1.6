module KeyValueStore

open System

type Pair<'a, 'b> = ('a * 'b)
type KVS<'a, 'b> = ('a * 'b * DateTime) list

let mutable dt: DateTime option = None

let now () = match dt with Some dt -> dt | None -> DateTime.Now

let empty = []

let eq key (k, _, _) = k = key

let putWithDt k v dt (kvs: KVS<_, _>) =
  (k, v, dt) :: (kvs |> List.filter (eq k >> not))

let put k v kvs =
  kvs |> putWithDt k v (now())

let putAll xs kvs =
  xs |> List.fold (fun st (k, v) -> st |> put k v) kvs

let init kvs = empty |> putAll kvs

let get key kvs =
  (kvs: KVS<_, _>)
  |> List.tryFind (eq key)
  |> Option.map (fun (_, v, _) -> v)

let delete key (kvs: KVS<_, _>) =
  kvs |> List.filter(eq key >> not) 

let deleteUntil t (kvs: KVS<_, _>) =
  kvs |> Seq.takeWhile (fun (_, _, dt) -> t <= dt) |> Seq.toList

let toStr kvs =
  sprintf "%A" (kvs: KVS<_, _>)

let toStrFrom t kvs =
  kvs |> deleteUntil t |> toStr

let dump kvs =
  do printf "%s" (kvs |> toStr)

let dumpFrom t kvs =
  do printf "%s" (kvs |> toStrFrom t)