module KeyValueStoreScenario

open NaturalSpec
open NUnit.Framework
open System
open KeyValueStore

let defaultDt = DateTime(2011, 7, 31)

[<SetUpFixture>]
type SetUp() =
  [<SetUp>]
  member x.setup () =
    KeyValueStore.dt <- Some defaultDt

module 生成 =
  [<Scenario>]
  let 空のKVSが生成できる() =
    Given KeyValueStore.empty
    |> It should equal []
    |> Verify

  [<Scenario>]
  let ペアを複数登録したKVSが生成できる() =
    Given [(1, "a"); (2, "b")]
    |> When KeyValueStore.init
    |> It should equal [(2, "b", defaultDt); (1, "a", defaultDt)]
    |> Verify

module 登録 =
  [<Example(1, "b")>]
  [<Example(2, "a")>]
  let ``空のKVSにペアを登録できる`` k v =
    Given KeyValueStore.empty
    |> When put k v
    |> It should equal [k, v, defaultDt]
    |> Verify

  [<Example(2, "b")>]
  [<Example(3, "c")>]
  let ``KVS内に存在しないキーのペアを登録すると、登録したペアが追加されたKVSが返る`` k v =
    Given KeyValueStore.init [1, "a"]
    |> When put k v
    |> It should equal [k, v, defaultDt; 1, "a", defaultDt]
    |> Verify

  [<Scenario>]
  let ``KVS内に存在するキーのペアを登録すると、値が更新される``() =
    Given KeyValueStore.init [1, "a"; 2, "b"; 3, "c"]
    |> When put 2 "hoge"
    |> It should equal [2, "hoge", defaultDt; 3, "c", defaultDt; 1, "a", defaultDt]
    |> Verify

  [<Scenario>]
  let 時刻を指定して登録できる() =
    Given KeyValueStore.empty
    |> When putWithDt 1 "hoge" (DateTime(2010, 7, 30))
    |> It should equal [1, "hoge", DateTime(2010, 7, 30)]
    |> Verify

module 複数登録 =
  [<Scenario>]
  let 複数登録できる() =
    Given KeyValueStore.init ["hoge", "piyo"; "foo", "bar"]
    |> When putAll ["a", "aaa"; "b", "bbb"]
    |> It should equal ["b", "bbb", defaultDt; "a", "aaa", defaultDt; "foo", "bar", defaultDt; "hoge", "piyo", defaultDt]
    |> Verify

  [<Scenario>]
  let 既に存在するキーは値が上書きされる() =
    Given KeyValueStore.init [1, "hoge"; 2, "piyo"; 3, "foo"; 4, "bar"]
    |> When putAll [1, "aaa"; 3, "bbb"]
    |> It should equal [3, "bbb", defaultDt; 1, "aaa", defaultDt; 4, "bar", defaultDt; 2, "piyo", defaultDt]
    |> Verify

  [<Scenario>]
  let 指定した引数内にキーの重複がある場合後勝ち() =
    Given KeyValueStore.empty
    |> When putAll [1, "aaa"; 2, "bbb"; 3, "ccc"; 2, "hoge"]
    |> It should equal [2, "hoge", defaultDt; 3, "ccc", defaultDt; 1, "aaa", defaultDt]
    |> Verify

module 取得 =
  [<Scenario>]
  let 空のKVSから取得するとNoneが返る() =
    Given KeyValueStore.empty
    |> When get "hoge"
    |> It should equal None
    |> Verify

  [<Scenario>]
  let 存在するキーを指定して取得するとSomeに包まれた値が取得できる() =
    Given KeyValueStore.init [1, 10]
    |> When get 1
    |> It should equal (Some 10)
    |> Verify

  [<Scenario>]
  let 存在しないキーを指定して取得するとNoneが返る() =
    Given KeyValueStore.init [1, 10]
    |> When get 10
    |> It should equal None 
    |> Verify

module 削除 =
  [<Scenario>]
  let 空のKVSで削除しても空のまま() =
    Given KeyValueStore.empty
    |> When delete "hoge"
    |> It should equal []
    |> Verify

  [<Scenario>]
  let 存在するキーを指定して削除すると取り除く() =
    Given KeyValueStore.init [1, "a"; 2, "b"; 10, ""]
    |> When delete 2
    |> It should equal [10, "", defaultDt; 1, "a", defaultDt]
    |> Verify

  [<Scenario>]
  let 指定時間より前のペアを削除できる() =
    Given KeyValueStore.empty
          |> putWithDt 0 0 (DateTime(2011, 7,31))
          |> putWithDt 1 0 (DateTime(2011, 8, 31))
          |> putWithDt 2 0 (DateTime(2011, 9, 30))
    |> When deleteUntil (DateTime(2011, 9, 1))
    |> It should equal [2, 0, DateTime(2011, 9, 30)]
    |> Verify

  [<Scenario>]
  let 指定時間は削除されない() =
    Given KeyValueStore.empty
          |> putWithDt 0 0 (DateTime(2011, 7,31))
          |> putWithDt 1 0 (DateTime(2011, 8, 31))
          |> putWithDt 2 0 (DateTime(2011, 9, 30))
    |> When deleteUntil (DateTime(2011, 8, 31))
    |> It should equal [2, 0, DateTime(2011, 9, 30); 1, 0, DateTime(2011, 8, 31)]
    |> Verify

module 文字列化 =
  [<Scenario>]
  let 空のKVSを文字列化できる() =
    Given KeyValueStore.empty
    |> When toStr
    |> It should equal "[]"
    |> Verify

  [<Scenario>]
  let ペアを一つ含むKVSを文字列化できる() =
    Given KeyValueStore.init ["a", 10]
    |> When toStr
    |> It should equal @"[(""a"", 10, 2011/07/31 0:00:00)]"
    |> Verify

  [<Scenario>]
  let ``新→旧の順番で文字列化される``() =
    Given KeyValueStore.init ["a", 10; "b", 20]
    |> When toStr
    |> It should equal @"[(""b"", 20, 2011/07/31 0:00:00); (""a"", 10, 2011/07/31 0:00:00)]"
    |> Verify

  [<Scenario>]
  let 時刻を指定して文字列化できる() =
    Given KeyValueStore.empty
          |> putWithDt 0 0 (DateTime(2011, 7, 31))
          |> putWithDt 1 0 (DateTime(2011, 8, 1))
          |> putWithDt 2 0 (DateTime(2011, 8, 2))
    |> When toStrFrom (DateTime(2011, 8, 1))
    |> It should equal @"[(2, 0, 2011/08/02 0:00:00); (1, 0, 2011/08/01 0:00:00)]"
    |> Verify
