[!include[Welcome to RDO.Net](_rdo_net_summary.md)]

[!include[RDO.Net License](_rdo_net_license.md)]

## Why RDO.Net

Enterprise application, typically backed by a relational database, has decades of history. Today's enterprise applications are unnecessarily complex and heavyweight, due to the following technical constraints:

* [Object-Relational Mapping (ORM, O/RM, and O/R mapping tool)](https://en.wikipedia.org/wiki/Object-relational_mapping), as the core of enterprise applications, is still [The Vietnam of Computer Science](http://blogs.tedneward.com/post/the-vietnam-of-computer-science/). Particularly, these difficulties are referred to as the [object-relational impedance mismatch](https://en.wikipedia.org/wiki/Object-relational_impedance_mismatch).
* Database testing, still stays on principles and guidelines. No widely practical use yet. Refactoring an enterprise
application is time consuming and error prone.
* Separation of the graphical user interface from the business logic or back-end logic (the data model), is still a challenge task. Frameworks such as [MVVM](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel) exists, but it's far from ideal: it will hit the wall when dealing with complex layout or complex interactivity; refactoring UI logic is still error prone, etc.

The above challenges impose great burdens for developing and further changing an enterprise application. Many frameworks are trying to solve these problems however they are all far from ideal. RDO.Net is the only solution to these problems in an integral, not an after-thought way (strongly recommended reading through):

* <xref:enterprise_application_the_right_way>
* <xref:orm_data_access_the_right_way>
* <xref:data_presentation_the_right_way>

In the end, your application follows your business in a no-more-no-less basis - it adapts to your business, not vice versa:

* Your application is 100% strongly typed from database to GUI, with clean code in C#/VB.Net. No fancy HQL, XAML or any other DSL - traditional problems are solved in a traditional way.
* Your data access is best balanced for both performance and maintainability because you're writing native SQL using C#/VB.Net.
* A one-for-all, fully customizable data presenter is provided to handle view states including layout, data binding and data validation, all in clean C#/VB.Net code. Most complex controls such as ListBox, TreeView, DataGrid are eliminated. You have the greatest control over your UI because you're dealing with basic control such as label and text box directly.
* JSON serialization/deserialization is a first class citizen - better performance because no reflection required.
* Database testing and deployment is a first class citizen. You can easily mock part of database with testing data, perform two way conversion between DataSet C#/VB.Net code and the database, right in Visual Studio. C#/VB.Net code is everything you need to commit in your source control repo.
* And much more with a lightweight runtime - you only need to add several dlls into your application, only hundreds or even tens of KBs in size!

## A Taste of RDO.Net

A fully featured sample application, [AdventureWorksLT](https://github.com/DevZest/AdventureWorksLT), together with others, is provided to demonstrate the use of RDO.Net:

![image](/images/samples_adventureworkslt.wpfapp.jpg)

### The Model

# [C#](#tab/cs)

[!code-csharp[SalesOrderDetail](../../../samples/AdventureWorksLT.SqlServer/AdventureWorksLT/SalesOrderDetail.cs)]

# [VB.Net](#tab/vb)

[!code-vb[SalesOrderDetail](../../../samples.vb/AdventureWorksLT.SqlServer/AdventureWorksLT/SalesOrderDetail.vb)]

***

The code of model can be manipulated in Model Visualizer tool window in Visual Studio:

![image](/images/SalesOrderDetailModelVisualizer.jpg)

### The Database

# [C#](#tab/cs)

[!code-csharp[Db](../../../samples/AdventureWorksLT.SqlServer/AdventureWorksLT/Db.cs)]

# [VB.Net](#tab/vb)

[!code-vb[Db](../../../samples.vb/AdventureWorksLT.SqlServer/AdventureWorksLT/Db.vb)]

***

The code of database can be manipulated via Db Visualizer tool window in Visual Studio:

![image](/images/AdventureWorksLTDbVisualizer.jpg)

### Data Access (CRUD)

# [C#](#tab/cs)

[!code-csharp[DbApi](../../../samples/AdventureWorksLT.SqlServer/AdventureWorksLT/Db.Api.cs#SalesOrderCRUD)]

# [VB.Net](#tab/vb)

[!code-vb[DbApi](../../../samples.vb/AdventureWorksLT.SqlServer/AdventureWorksLT/Db.Api.SalesOrder.vb)]

***

### Data Presentation

# [C#](#tab/cs)

[!code-csharp[DetailPresenter](../../../samples/AdventureWorksLT.WpfApp/SalesOrderWindow.DetailPresenter.cs)]

# [VB.Net](#tab/vb)

[!code-vb[DetailPresenter](../../../samples.vb/AdventureWorksLT.WpfApp/SalesOrderWindow.DetailPresenter.vb)]

***

This will produce the following data grid UI, with foreign key lookup and paste append from clipboard:

![image](/images/SalesOrderDetailUI.jpg)

### Mock Database for Testing

# [C#](#tab/cs)

```csharp
public sealed class MockSalesOrder : DbMock<Db>
{
    public static Task<Db> CreateAsync(Db db, IProgress<DbInitProgress> progress = null, CancellationToken ct = default(CancellationToken))
    {
        return new MockSalesOrder().MockAsync(db, progress, ct);
    }

    // This method is generated by a tool
    private static DataSet<SalesOrderHeader> Headers()
    {
        DataSet<SalesOrderHeader> result = DataSet<SalesOrderHeader>.Create().AddRows(4);
        SalesOrderHeader _ = result._;
        _.SuspendIdentity();
        _.SalesOrderID[0] = 1;
        _.SalesOrderID[1] = 2;
        ...
        _.ResumeIdentity();
        return result;
    }

    // This method is generated by a tool
    private static DataSet<SalesOrderDetail> Details()
    {
        DataSet<SalesOrderDetail> result = DataSet<SalesOrderDetail>.Create().AddRows(32);
        SalesOrderDetail _ = result._;
        _.SuspendIdentity();
        ...
        _.SalesOrderDetailID[0] = 1;
        _.SalesOrderDetailID[1] = 2;
        ...
        _.ResumeIdentity();
        return result;
    }

    protected override void Initialize()
    {
        // The order of mocking table does not matter, the dependencies will be sorted out automatically.
        Mock(Db.SalesOrderDetail, Details);
        Mock(Db.SalesOrderHeader, Headers);
    }
}
```

# [VB.Net](#tab/vb)

```vb
Public Class MockSalesOrder
    Inherits DbMock(Of Db)

    Public Shared Function CreateAsync(db As Db, Optional progress As IProgress(Of DbInitProgress) = Nothing, Optional ct As CancellationToken = Nothing) As Task(Of Db)
        Return New MockSalesOrder().MockAsync(db, progress, ct)
    End Function

    ' This method is generated by a tool
    Private Shared Function Headers() As DataSet(Of SalesOrderHeader)
        Dim result As DataSet(Of SalesOrderHeader) = DataSet(Of SalesOrderHeader).Create().AddRows(4)
        Dim x As SalesOrderHeader = result.Entity
        x.SuspendIdentity()
        x.SalesOrderID(0) = 1
        x.SalesOrderID(1) = 2
        ...
        x.ResumeIdentity()
        Return result
    End Function

    ' This method is generated by a tool
    Private Shared Function Details() As DataSet(Of SalesOrderDetail)
        Dim result As DataSet(Of SalesOrderDetail) = DataSet(Of SalesOrderDetail).Create().AddRows(32)
        Dim x As SalesOrderDetail = result.Entity
        x.SuspendIdentity()
        ...
        x.SalesOrderDetailID(0) = 1
        x.SalesOrderDetailID(1) = 2
        x.ResumeIdentity()
        ...
        Return result
    End Function

    Protected Overrides Sub Initialize()
        Mock(Db.SalesOrderHeader, Function() Headers())
        Mock(Db.SalesOrderDetail, Function() Details())
    End Sub
End Class
```

***

Testing data code is generated from database:

![image](/images/SalesOrderDetailMockDb.jpg)

## Getting Started

It's highly recommended to start with our [step by step tutorial](xref:tutorial_get_started).