# Fluent Updater - Api documentation

Namespace: <span style="color:#75B6E7"> MekUpdater.UpdateBuilder</span>  
Assembly: <span style="color:#75B6E7"> MekUpdater.dll</span>

- This part of the updater uses fluent api style where you can chain commands nicely
- Firstly UpdateBuilder builds update and then update process can be run

## Examples

- Start building you update by initializing new Update builder
- Update builder is situated in "MekUpdater.UpdateBuilder" namespace
- You may also need to use MekPathLibrary nuget package

<br/>

### Run default update

Program.cs

```csharp
using MekUpdater.UpdateBuilder;

// info from your repository where all updates are read
string repositoryOwnerName = "my_name";
string repositoryName = "my_cool_repository";


// Initialize new update with Update builder

Update update = UpdateBuilder.Create(repositoryOwnerName, repositoryName)   // initialize new
                             .RunUpdate()   // update runs always
                             .StartSetup()  // starts setup.exe
                             .TidiesUp()    // cleans all created files used by update process
                             .Build();      // build update

// Then you can run update asynchronously
await update.RunAsync();

```

<br/>

### Update Arguments

- How update is run can be changed by chaining different methods or by passing optional arguments
- Update has five main states where different parts can be manipulated

<br/>

#### State 1

- Update is

#### State 2

#### State 3

#### State 4

#### State 5

- Update ready to run
