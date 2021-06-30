
[![Build status](https://ci.appveyor.com/api/projects/status/qhmposba71n8lvpo/branch/master?svg=true)](https://ci.appveyor.com/project/icarus-consulting/brix/branch/master)
[![codecov](https://codecov.io/gh/icarus-consulting/BriX/branch/master/graph/badge.svg?token=4SZ8XXKXDU)](https://codecov.io/gh/icarus-consulting/BriX)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](http://makeapullrequest.com)
[![Commitizen friendly](https://img.shields.io/badge/commitizen-friendly-brightgreen.svg?style=flat-square)](http://commitizen.github.io/cz-cli/)
[![EO principles respected here](http://www.elegantobjects.org/badge.svg)](http://www.elegantobjects.org)

## Repo Guidelines

Main responsible for this reposotory is [icaOBU](https://github.com/icaOBU).
Please request in every single PR a review from him. 

He will try to review the PR´s within **1 week** and merge applied PR´s within **2 weeks** with a new release. Main review day is monday.

# BriX

Create data structures using *object oriented* bricks.

This library follows all the principles suggested in the two "[Elegant Objects](https://www.amazon.de/Elegant-Objects-Yegor-Bugayenko/dp/1519166915)" books.

It adopts the approach of the blog article [printers instead of getters](https://www.yegor256.com/2016/04/05/printers-instead-of-getters.html).

```csharp
var menu =
    new BxBlock("menu",
    	new BxBlockArray("meals", "meal",
            new BxBlock(
            	new BxProp("name", "Pizza Funghi"),
                new BxProp("price", "9.50€")
            ),
            new BxBlock(
        	new BxProp("name", "Burger Helene with Fritten"),
	        new BxProp("price", "10.50€")
    	    )
    	),
	new BxBlockArray("drinks", "drink",
    	    new BxBlock(
        	new BxProp("name", "Beer"),
            	new BxProp("price", "2.50€")
	    ),
    	    new BxBlock(
        	new BxProp("name", "Beer"),
            	new BxProp("price", "35.00€")
	    )
 	)
    );
```

And decide later which format you need:

## Print as XML

```csharp
Console.WriteLine(
    menu.Print(new XmlMedia())
);
```

Will give you

```xml
<menu>
  <meals>
    <meal>
      <Name>Pizza Funghi</Name>
      <Price>9.50€</Price>
    </meal>
    <meal>
      <Name>Burger Helene with Fritten</Name>
      <Price>10.50€</Price>
    </meal>
  </meals>
  <Drinks>
    <Drink>
      <Name>Beer</Name>
      <Price>2.50€</Price>
    </Drink>
    <Drink>
      <Name>Beer</Name>
      <Price>35.00€</Price>
    </Drink>
  </Drinks>
</menu>
```

## Print as Json

```csharp
Console.WriteLine(
    menu.Print(new JsonMedia())
);
```

Will give you

```json
{
  "meals": [
    {
      "Name": "Pizza Funghi",
      "Price": "9.50€"
    },
    {
      "Name": "Burger Helene with Fritten",
      "Price": "10.50€"
    }
  ],
  "Drinks": [
    {
      "Name": "Beer",
      "Price": "2.50€"
    },
    {
      "Name": "Beer",
      "Price": "35.00€"
    }
  ]
}
```



## Lazy Data aggregation

You can build Brix which will aggregate data only when printed:

```csharp
var report =
    new BxBlock("Weather Report",
    	new BxBlock(
	    new BxProp("Temperature", () => weatherServer.Degrees("Berlin").AsDouble()) //not yet read
	)
    );

report.Print(new XmlMedia()); //Temperature is read while printing
```



## Usage

With BriX you can build data structures without deciding which format someone should use to work with it. This has advantages for example when designing a web-API: When you add a new usecase, you design the output using brix and can leave the decision if the user needs xml or json to the http request header.

```csharp
var report = 
    new BxBlock("report", 
        new BxProp("All good", "Yes")
    );

if(new Header.Of("accept", httpRequest) == "application/xml")
{
    return myReport.Print(new XmlMedia()).ToString();
}
else if(new Header.Of("accept", httpRequest) == "application/json")
{
    return myReport.Print(new JsonMedia()).ToString();
}
```

 In other scenarios you may deliver a non printed BriX block as payload of another object, and if the user just needs the head of your object, but not the payload, computation time can be saved.

```csharp
//Someone sends this:
public void Send()
{
    var signal =
    	new SignalOf("Event", "Something is on fire",
            new BxArray("Burning things"
            	new ListOf<string>(
                    () => BurningThings()
	        )
    	    )
	);
}

public void BurningThings()
{
    var burning = new List<string>();
    if(inventory.Cat().IsBurning())
    {
        burning.Add("The cat");
    }
}


//...control flow happens in between...

//Someone receives:
var received = signal;

if(signal.Prop("event" == "Something exploded")) //is false in this example
{
    var payload = 
        signal.Payload()
            .Print(new XmlMedia()); //Only when you print, the burning things would be inspected.
}
```


## Data Conventions
BriX can print XML and Json, and more media formats can be added by implementing the IMedia<TOutput> interface.
Because XML and Json have different feature sets, BriX limits these features to ensure BriX are compatible to both xml and json. 

### Block names
Every Block must have a name:

```
//Xml
<root>
   <branch>My Branch</branch>
</root>

//Json
{
   "branch": "My branch"
}

//C#
var brix =
    new BxBlock(
        "root", //name must be specified.
	    new BxProp("branch", "My Branch")
    );
```

Obviously, the name "root" is lost when printing to Json. But Brix enforces that you specify it, because it is needed for Xml.

### Simple Arrays

The same is the case for arrays:

```
//Xml
<shopping-list>
    <fruits>
    	<fruit>Apple</fruit>
    	<fruit>Banana</fruit>		
    </fruits>
</shopping-list>

//Json
{
    "fruits":
    [
        "Apple",
        "Banana"
    ]
}

//C#
var list =
    new BxBlock("shopping-list",
    	new BxArray("fruits", "fruit", //"fruit" as name for entries must be specified
    	    "Apple", 
    	    "Banana"
        )
    );
```

### Complex Arrays
And for complex arrays:
```
//Xml
<shopping-list>
	<fruits>
		<fruit>
			<name>Apple</name>
			<weight>500</weight>
		</fruit>
		<fruit>
			<name>Banana</name>
			<weight>300</weight>
		</fruit>
	</fruits>
</shopping-list>


//Json
{
	"fruits": [
		{
			"name": "Apple",
			"weight": "500"
		},
		{
			"name": "Banana",
			"weight": "300"
		}
	]
}

//C#
var list =
    new BxBlock("shopping-list",
	    new BxBlockArray("fruits", "fruit", //"fruit" as name for entries must be specified
	        new BxBlock(
	            new BxProp("name", "Apple"), 
	            new BxProp("weight", "500")
            ), 
            new BxBlock(
                new BxProp("name", "Banana"), 
                new BxProp("weight", "300")
            )
        )
    );
```

### Xml Attributes
Json does not support attributes, so they are not included in BriX objects. If you need them, you might implement or extend the JsonMedia interface to support them and write BriX objects.
### BxMap
Easy way for adding mutiple Props by KeyValuePairs. Note: no duplicated keys are allowed!
```
//xml
<person>
    <name>George</name>
    <age>23</age>
    <gender>male</gender>
</person>

//json
{
    "name": "George",
    "age": "23",
    "gender": "male"
}

//C#
new BxBlock(
    "person",
    new BxMap(
        "name", "George",
        "age", "23",
        "gender", "male"
    )
)
```
## Conditional BriX
Instead of using large if/else constructs:

```csharp
new BxBlock("Todos",
    new BxConditional(() => Now.IsDay(),
        new BxProp("Todo", "Daylight dependent tasks")
    ),
    new BxConditional(() => Now.IsNight(),
        new BxProp("Todo", "Nightly tasks")
    )
)
```

## Rebuildable Brix
For sending a BriX e.g. as a response of a request, it´s possible to print a BriX to a RebuildMedia. 

```csharp
    var response =
        new BxBlock("Weather Report",
    	    new BxBlock(
	        new BxProp("Temperature", () => weatherServer.Degrees("Berlin").AsDouble())
	    )
    );

report.Print(new RebuildMedia());
return report.Content(); // the byte[] of the BriX as xml 
```

At the requesting side, you can reconstruct the BriX with BxRebuilt
```csharp
var brix = response.Body()
var rebuildedBrix = new BxRebuild(brix);
```



