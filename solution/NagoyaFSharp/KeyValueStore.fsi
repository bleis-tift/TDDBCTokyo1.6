module KeyValueStore

open System

type Pair<'a, 'b> = ('a * 'b)
type KVS<'a, 'b> = ('a * 'b * DateTime) list

// 公開API
val empty: 'a list

val init: Pair<'a, 'b> list -> KVS<'a, 'b> when 'a: equality

val put: 'a -> 'b -> KVS<'a, 'b> -> KVS<'a, 'b> when 'a: equality

val putAll: Pair<'a, 'b> list -> KVS<'a, 'b> -> KVS<'a, 'b> when 'a: equality

val get: 'a -> KVS<'a, 'b> -> 'b option when 'a: equality

val delete: 'a -> KVS<'a, 'b> -> KVS<'a, 'b> when 'a: equality

val deleteUntil: DateTime -> KVS<'a, 'b> -> KVS<'a, 'b>

val dump: KVS<_, _> -> unit

val dumpFrom: DateTime -> KVS<_, _> -> unit

// 以下テスト用
val mutable internal dt: DateTime option

val internal putWithDt:
  'a -> 'b -> DateTime -> KVS<'a, 'b> -> KVS<'a, 'b>
    when 'a: equality

val internal toStr: KVS<_, _> -> string

val internal toStrFrom: DateTime -> KVS<_, _> -> string