## Synopsis

A simple .NET Core wrapper for ElasticSearch.Net

## Code Example

**Connecting to a node(s)**

```csharp
var client = new ElasticLiteClient("http://myserver:9200", "http://myserver:9201");
```

**Implement IElasticPoco interface**

```csharp
public class MyPoco : IElasticPoco
{
    public string Id { get; set; }
    public string Type { get; set; }
	public string Index { get; set; }
	public double Score { get; set; }
	public string TestText { get; set; }
	public int TestInteger { get; set; }
	public double TestDouble { get; set; }
	public DateTime TestDateTime { get; set; }
	public bool TestBool { get; set; }
	public string[] TestStringArray { get; set; }
}
```

## Query Examples

**Search Example**

```csharp
var client = new ElasticLiteClient("http://myserver:9200");
var query = Search<MyPoco>
	.In("mypocoindex", "mypoco")
    .Term("TestText", "ABCDEFG");

var document = client.ExecuteSearch(query);
```

**Index Example**

```csharp
var client = new ElasticLiteClient("http://myserver:9200");
var query = Index<MyPoco>
	.Document(poco);
	
client.ExecuteIndex(query);
```

**Update Example**

```csharp
var client = new ElasticLiteClient("http://myserver:9200");
poco.LastChanged = DateTime.UtcNow;
var query = Update<MyPoco>
	.Document(poco);
	
client.ExecuteUpdate(query);
```

**Delete Example**

```csharp
var client = new ElasticLiteClient("http://myserver:9200");
var query = Delete<MyPoco>
	.Document(poco);
	
client.ExecuteDelete(query);
```