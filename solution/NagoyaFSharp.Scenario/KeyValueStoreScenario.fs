module KeyValueStoreScenario

open NaturalSpec
open KeyValueStore

[<Scenario>]
let 空のKVSが生成できる() =
  Given ()
  |> When (fun () -> KeyValueStore.empty)
  |> It should equal []
  |> Verify

[<Scenario>]
let ``1をキーに、"b"を値に空のKVSに追加すると、[(1, "b")]が返る``() =
  Given KeyValueStore.empty
  |> When put 1 "b"
  |> It should equal [(1, "b")]
  |> Verify

[<Scenario>]
let ``2をキーに、"a"を値を空のKVSに追加すると、[(2, "a")]が返る``() =
  Given KeyValueStore.empty
  |> When put 2 "a"
  |> It should equal [(2, "a")]
  |> Verify