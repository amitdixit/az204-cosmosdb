using CosmosDbApp;


//Creation only needs to be doe at once
// await CosmosManager.CreateCosmosDb();

// Add Single Item to Cosmos
//await CosmosManager.AddItem();

//Add Bulk Item To Cosmos
//await CosmosManager.AddItemsInBulk();

//Read Data From Cosmos
//await CosmosManager.ReadDataFromCosmos();


//Update Data in Cosmos DB
//await CosmosManager.UpdateItem();

//Delete Data in Cosmos DB
//await CosmosManager.DeleteItem();

//Execute Stored Procedure
//await CosmosManager.ExecuteStoredProcedure();

// Add Single Item to Cosmos using Stored Procedure
//await CosmosManager.AddItemUsingStoredProcedure();

//Pre Trigger
//await CosmosManager.AddItemPreTrigger();

// Add Embedded Item to Cosmos
//await CosmosManager.AddEmbeddedItem();

//Cosmos Table API Demo
await CosmosManager.AddItemToTable();