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
let ``空のKVSにキーと値をputすると、それのみを含むKVSが返る`` k v =
  Given KeyValueStore.empty
  |> When put k v
  |> It should equal [k, v]
  |> Verify

[<Example(2, "b")>]
let ``(1, "a")のみを含むKVSに1以外のキーと値をputすると、putしたキーと値が追加されたKVSが返る`` k v =
  Given KeyValueStore.empty |> put 1 "a"
  |> When put k v
  |> It should equal [1, "a"; k, v]
  |> Verify