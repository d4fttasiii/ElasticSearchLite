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

After creating a new ElasticLiteClient object, you can build up your Search object which requires an index and a type name to use the Search API from ElasticSearch. After this the search can be specified with different methods like: Term, Match, Range, Include, Exclude.

```csharp
var client = new ElasticLiteClient("http://myserver:9200");
var query = Search<MyPoco>
    .In("mypocoindex", "mypoco")
    .Term("TestText", "ABCDEFG");

var document = client.ExecuteSearch(query);
```

**Index Example**

The add a document to ElasticSearch using a POCO object, the POCO will require the index and type name which will be used by the Index API from ElasticSearch. After the request is completed the poco's Id property will be updated if it hasn't been set in the before the request, in which case the given Id will be set in ElasticSearch.

```csharp
var client = new ElasticLiteClient("http://myserver:9200");
var poco = new MyPoco { 
    Index = "mypocoindex", 
    Type = "mypoco", 
    TestText = "ElasticSearch is awesome!" 
};
var query = Index<MyPoco>
    .Document(poco);
	
client.ExecuteIndex(query);
```

**Update Example**

Update can be used to persist POCO changes to ElasticSearch as in seen in this example below. The POCO does require a valid Id property. That also goes for Index and Type like previously.

```csharp
var client = new ElasticLiteClient("http://myserver:9200");
poco.LastChanged = DateTime.UtcNow;
var query = Update<MyPoco>
    .Document(poco);
	
client.ExecuteUpdate(query);
```

**Delete POCO Example**

Delete will remove one existing document from ElasticSearch when it's constructed from a POCO.

```csharp
var client = new ElasticLiteClient("http://myserver:9200");
var query = Delete<MyPoco>
    .Document(poco);
	
client.ExecuteDelete(query);
```

**Delete Query Example**

Delete can also be used as a query to remove multiple documents from ElasticSearch if necessary.

```csharp
var client = new ElasticLiteClient("http://myserver:9200");
var query = Delete<MyPoco>
    .From("mypocoindex", "mypoco")
    .Term("TestText", "ABCDEFG");
	
client.ExecuteDelete(query);
```
