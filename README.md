## Synopsis

A simple .NET Core wrapper for ElasticSearch.Net

## Code Example

**Connecting to a node(s)**

```csharp
var client = new ElasticLiteClient("http://myserver:9200", "http://myserver:9201");
```

**Implement IElasticPoco interface for your pocos.**

```csharp
public class MyPoco : IElasticPoco
{
    public string Id { get; set; }
    public string Type { get; set; }
    public string Index { get; set; }
    public double? Score { get; set; }
    public Tag Tag { get; set; }
    public Position Position { get; set; }
    public string Name { get; set; }
    public DateTime LastModified { get; set; }
}
public class Tag
{
    public string Name { get; set; }
    public string Summary { get; set; }
}
public class Position
{
    public double X { get; set; }
    public double Y { get; set; }
}
```

## Query Examples

**Search Example**

After creating a new ElasticLiteClient object, you can build up your Search object which requires an index to use the Search API from ElasticSearch. After this the search can be specified with different methods like: Term, Match, Range, Sort, Take, Skip.

```csharp
var client = new ElasticLiteClient("http://myserver:9200");

// Term query example
var pocosFoundWithTerm = Search
    .In("mypocoindex")
    .Return<MyPoco>()
    .Term(p => p.Name, "ABCDEFG")
    .Sort(p => p.LastModified, ElasticSortOrders.Descending)
    .Take(20)
    .Skip(0)
    .ExecuteWith(client);
    
// Match query example
var pocosFoundWithMatch = Search
    .In("mypocoindex")
    .Return<MyPoco>()
    .Match(p => p.Name, "ABCD EFGH")
    	.Operator(ElasticOperators.Or)
    .Sort(p => p.LastModified)
    	.ThenBy(p => p.Score, ElasticSortOrders.Descending)
    .ExecuteWith(client);
```

**Index Example**

The add a document to ElasticSearch using a POCO object, the POCO will require the index and type name which will be used by the Index API from ElasticSearch. After the request is completed the poco's Id property will be updated if it hasn't been set in the before the request, in which case the given Id will be set in ElasticSearch.

```csharp
var client = new ElasticLiteClient("http://myserver:9200");
var poco = new MyPoco 
{ 
    Index = "mypocoindex", 
    Type = "mypoco", 
    Name = "ElasticSearch is awesome!",
	LastModified = DateTime.Now()
};

Index.Document(poco)
    .ExecuteWith(client);
```

**Update Example**

Update can be used to persist POCO changes to ElasticSearch as in seen in this example below. The POCO does require a valid Id property. That also goes for Index and Type like previously.

```csharp
var client = new ElasticLiteClient("http://myserver:9200");
poco.LastModified = DateTime.UtcNow;

Update
    .Document(poco)
    .ExecuteWith(client);
```

**Delete POCO Example**

Delete will remove one existing document from ElasticSearch when it's constructed from a POCO.

```csharp
var client = new ElasticLiteClient("http://myserver:9200");

var deleteDocuments = Delete
    .Document(poco)
    .ExecuteWith(client);
```

**Delete Query Example**

Delete can also be used as a query to remove multiple documents from ElasticSearch if necessary.

```csharp
var client = new ElasticLiteClient("http://myserver:9200");

var deleteDocuments = Delete
    .From("mypocoindex")
    .Documents<MyPoco>()
    .Term(p => p.Name, "ABCDEFG")
    .ExecuteWith(client);
```
