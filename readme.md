
[![Build status](https://ci.appveyor.com/api/projects/status/qhmposba71n8lvpo/branch/master?svg=true)](https://ci.appveyor.com/project/icarus-consulting/brix/branch/master)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](http://makeapullrequest.com)
[![Commitizen friendly](https://img.shields.io/badge/commitizen-friendly-brightgreen.svg?style=flat-square)](http://commitizen.github.io/cz-cli/)
[![EO principles respected here](http://www.elegantobjects.org/badge.svg)](http://www.elegantobjects.org)

# ZiZZi

Create data structures using *object oriented* blocks. 

The code follows all the principles suggested in the two "[Elegant Objects](https://www.amazon.de/Elegant-Objects-Yegor-Bugayenko/dp/1519166915)" books.

It adopts the approach of the blog article [printers instead of getters](https://www.yegor256.com/2016/04/05/printers-instead-of-getters.html).

## Origin
This is a fork of [BriX](https://github.com/icarus-consulting/BriX). This library offers more flexibility over the resulting output.

1. BriX' main goal was to express data in a way that fits json as well as xml. It achieved this by offering a limited set of structuring features:
For example, json does not have attributes, BriX does not support them. ZiZZi lets this choice open to the library user, as the interfaces can be used to introduce more types.

2. With ZiZZi you can transform to anonymous objects - only compiling the necessary properties - which can be handy for UI building.

3. ZiZZi can express typed content, also raw bytes and streams - BriX is limited to strings.


## This is how you declare a ZiZZi object
```csharp
var menu =
    new ZiBlock("menu",
    	new ZiBlockArray("meals", "meal",
            new ZiBlock(
            	new ZiProp("name", "Pizza Funghi"),
                new ZiProp("price", 9.50)
            ),
            new ZiBlock(
        	    new ZiProp("name", "Burger Helene with Fritten"),
	            new ZiProp("price", 10.50)
    	    )
    	),
	new ZiBlockArray("drinks", "drink",
    	    new ZiBlock(
        	    new ZiProp("name", "Beer"),
            	    new ZiProp("price", 2.50)
	        ),
    	    new ZiBlock(
        	    new ZiProp("name", "Beer"),
            	    new ZiProp("price", 35.00)
	        )
 	)
    );
```

## Form an Object

```csharp
    var obj =
        new ZiObject(
            new ZiProp("Name", "Mr.Object")
        ).Form(
            ObjectMatter.Fill(new { Name = "" }
        );

    //will give you an anonymous object with the properties filled by the defined ZiZZi.
    Assert.Equal("Mr.Object", obj.Name);

    //Currently, nested anonymous objects with properties are supported, as well as string lists.
    //Other types are work in progress.
);
```

## Express as XML

```csharp
Console.WriteLine(
    menu.Form(new XmlMatter())
);
```

Will give you

```xml
<menu>
  <meals>
    <meal>
      <Name>Pizza Funghi</Name>
      <Price>9.50</Price>
    </meal>
    <meal>
      <Name>Burger Helene with Fritten</Name>
      <Price>10.50</Price>
    </meal>
  </meals>
  <Drinks>
    <Drink>
      <Name>Beer</Name>
      <Price>2.50</Price>
    </Drink>
    <Drink>
      <Name>Beer</Name>
      <Price>35.00€</Price>
    </Drink>
  </Drinks>
</menu>
```

## Express as Json

```csharp
Console.WriteLine(
    menu.Form(new JsonMatter())
);
```

Will give you

```json
{
  "meals": [
    {
      "Name": "Pizza Funghi",
      "Price": 9.50
    },
    {
      "Name": "Burger Helene with Fritten",
      "Price": 10.50
    }
  ],
  "Drinks": [
    {
      "Name": "Beer",
      "Price": 2.50
    },
    {
      "Name": "Beer",
      "Price": 35.00
    }
  ]
}
```


## Lazy Data aggregation

You can build Blox which will aggregate data only when printed:

```csharp
var report =
    new ZiBlock("Weather Report",
    	new ZiBlock(
	    new ZiProp("Temperature", () => weatherServer.Degrees("Berlin").AsDouble()) //not yet read
	)
    );

report.Form(new XmlMatter()); //Temperature is read while printing
```



## Usage

With ZiZZi you can build data structures without deciding which format someone should use to work with it. This has advantages for example when designing a web-API: When you add a new usecase, you design the output using ZiZZi objects and can leave the decision if the user needs xml or json to the http request header.

```csharp
var report = 
    new ZiBlock("report", 
        new ZiProp("All good", "Yes")
    );

if(new Header.Of("accept", httpRequest) == "application/xml")
{
    return myReport.Form(new XmlMatter()).ToString();
}
else if(new Header.Of("accept", httpRequest) == "application/json")
{
    return myReport.Form(new JsonMatter()).ToString();
}
```

 In other scenarios you may deliver a non printed ZiZZi block as payload of another object, and if the user just needs the head of your object, but not the payload, computation time can be saved.

```csharp
//Someone sends this:
public void Send()
{
    var signal =
    	new SignalOf("Event", "Something is on fire",
            new ZiArray("Burning things"
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
            .Form(new XmlMatter()); //Only when you print, the burning things would be inspected.
}
```


## Data Conventions
ZiZZi can form XML and Json, and more media formats can be added by implementing the IMatter<TOutput> interface.
Because XML and Json have different feature sets, ZiZZi's offered objects use only these features to ensure Blox are compatible to both xml and json.
However, the IMatter interface can be implemented to support more features of a speccific format.

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
var blox =
    new ZiBlock(
        "root", //name must be specified.
	    new ZiProp("branch", "My Branch")
    );
```

Obviously, the name "root" is lost when printing to Json. But ZiZZi encourages that you specify it, because it is needed for Xml.

### Simple Arrays

The same is the case for lists:

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
    new ZiBlock("shopping-list",
    	new ZiValueList("fruits", "fruit", //"fruit" as name for entries must be specified
    	    "Apple", 
    	    "Banana"
        )
    );
```

### Complex Lists
And for block lists:
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
    new ZiBlock("shopping-list",
	    new ZiBlockArray("fruits", "fruit", //"fruit" as name for entries must be specified
	        new ZiBlock(
	            new ZiProp("name", "Apple"), 
	            new ZiProp("weight", "500")
            ), 
            new ZiBlock(
                new ZiProp("name", "Banana"), 
                new ZiProp("weight", "300")
            )
        )
    );
```

### Xml Attributes
Json does not support attributes, so they are not included in Blox objects. If you need them, you might implement or extend the JsonMatter objects to support them and write Blox objects.
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
## Conditional Object
Instead of using large if/else constructs:

```csharp
new ZiBlock("Todos",
    new ZiConditional(() => Now.IsDay(),
        new ZiProp("Todo", "Daylight dependent tasks")
    ),
    new ZiConditional(() => Now.IsNight(),
        new ZiProp("Todo", "Nightly tasks")
    )
)
```


