module KeyValueStore

open System

// 公開API
val empty: 'a list

val init: ('a * 'b) list -> ('a * 'b * DateTime) list when 'a: equality

val put:
  'a ->
    'b -> ('a * 'b * DateTime) list -> ('a * 'b * DateTime) list
    when 'a: equality

val putAll:
  ('a * 'b) list ->
    ('a * 'b * DateTime) list -> ('a * 'b * DateTime) list
    when 'a: equality

val get: 'a -> ('a * 'b * 'c) list -> 'b option when 'a: equality

val delete: 'a -> ('a * 'b * 'c) list -> ('a * 'b * 'c) list when 'a: equality

val deleteUntil:
  'a -> seq<'b * 'c * 'a> -> ('b * 'c * 'a) list when 'a: comparison

val dump: 'a -> unit

val dumpFrom: 'a -> seq<'b * 'c * 'a> -> unit when 'a: comparison

// 以下テスト用
val mutable internal dt: DateTime option

val internal putWithDt:
  'a -> 'b -> 'c -> ('a * 'b * 'c) list -> ('a * 'b * 'c) list
    when 'a: equality

val internal toStr: 'a -> string

val internal toStrFrom: 'a -> seq<'b * 'c * 'a> -> string when 'a: comparison
