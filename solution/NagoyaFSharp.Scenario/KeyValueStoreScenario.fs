module KeyValueStoreScenario

open NaturalSpec
open KeyValueStore

[<Scenario>]
let 空のKVSが生成できる() =
  Given ()
  |> When (fun () -> KeyValueStore.empty)
  |> It should equal []
  |> Verify

[<Example(1, "b")>]
[<Example(2, "a")>]
let ``空のKVSにペアをputすると、それのみを含むKVSが返る`` k v =
  Given KeyValueStore.empty
  |> When put k v
  |> It should equal [k, v]
  |> Verify

[<Example(2, "b")>]
[<Example(3, "c")>]
let ``(1, "a")のみを含むKVSに重複しないペアをputすると、putしたペアが追加されたKVSが返る`` k v =
  Given KeyValueStore.empty |> put 1 "a"
  |> When put k v
  |> It should equal [k, v; 1, "a"]
  |> Verify

[<Scenario>]
let 空のKVSをtoStrで文字列化できる() =
  Given KeyValueStore.empty
  |> When toStr
  |> It should equal "[]"
  |> Verify

[<Scenario>]
let ペアを一つ含むKVSをtoStrで文字列化できる() =
  Given KeyValueStore.empty |> put "a" 10
  |> When toStr
  |> It should equal @"[(""a"", 10)]"
  |> Verify