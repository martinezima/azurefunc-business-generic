using System.Collections;
using AppBusinessGeneric.Application.Helpers;
using AppBusinessGeneric.Application.Models;
using Armanino.Integration.Utilities.Models;

namespace AppBusinessGeneric.Tests;


public class ConverterObjectHelperTest
{
    [Fact]
    public void CheckIfIsAList_SetFieldsWithElementsClassTypeValidAndSystemObject_RetunTrue()
    {
        // Arrange
        var systemObject = "SupplierConfigurationImportBatches";
        var fields = new List<Field>
        {
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].ErpSourceId",
                    Value = "Erp Source Id 1"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].Company",
                    Value = "Company Test 1"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].FirstName",
                    Value = "Miguel"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].LastName",
                    Value = "Martinez"
                }
            },
        };

        // Action
        var isAList = ConverterObjectHelper.CheckIfIsAList(
            fields,
            systemObject);
        
        // Assertion
        Assert.True(isAList);
    }
    [Fact]
    public void CheckIfIsAList_SetFieldsWithElementsClassTypeValidAndSystemObject_RetunFalse()
    {
        // Arrange
        var systemObject = "SupplierConfigurationImportBatches";
        var fields = new List<Field>
        {
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches.ErpSourceId",
                    Value = "Erp Source Id 1"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches.Company",
                    Value = "Company Test 1"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches.FirstName",
                    Value = "Miguel"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches.LastName",
                    Value = "Martinez"
                }
            },
        };

        // Action
        var isAList = ConverterObjectHelper.CheckIfIsAList(
            fields,
            systemObject);
        
        // Assertion
        Assert.False(isAList);
    }
    [Fact]
    public void CheckOverAllProperties_SetFieldsAsListType_ReturnListWithOneItem()
    {
        // Arrange
        var systemObject = "SupplierConfigurationImportBatches";
        var fields = new List<Field>
        {
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].ErpSourceId",
                    Value = "Erp Source Id 1"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].Company",
                    Value = "Company Test 1"
                }
            }
        };

        // Action
        var expectedResult = ConverterObjectHelper.CheckOverAllProperties(
            fields,
            systemObject,
            true);
        
        // Assertion
        Assert.Single(expectedResult);
        Assert.Equal(expectedResult[0].ErpSourceId, "Erp Source Id 1");
        Assert.Equal(expectedResult[0].Company, "Company Test 1");
    }
    [Fact]
    public void CheckOverAllProperties_SetFieldsAsListType_ReturnListWithTwoItems()
    {
        // Arrange
        var systemObject = "SupplierConfigurationImportBatches";
        var fields = new List<Field>
        {
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].ErpSourceId",
                    Value = "Erp Source Id 1"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].Company",
                    Value = "Company Test 1"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[1].ErpSourceId",
                    Value = "Erp Source Id 2"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[1].Company",
                    Value = "Company Test 2"
                }
            }
        };

        // Action
        var expectedResult = ConverterObjectHelper.CheckOverAllProperties(
            fields,
            systemObject,
            true);
        
        // Assertion
        Assert.NotNull(expectedResult);
        Assert.Equal(expectedResult[0].ErpSourceId, "Erp Source Id 1");
        Assert.Equal(expectedResult[0].Company, "Company Test 1");
        Assert.Equal(expectedResult[1].ErpSourceId, "Erp Source Id 2");
        Assert.Equal(expectedResult[1].Company, "Company Test 2");
    }
    [Fact]
    public void CheckOverAllProperties_SetFieldsAsListOfObjectWithNestedCollectionAsProperty_ReturnListWithOneItem()
    {
        // Arrange
        var systemObject = "SupplierConfigurationImportBatches";
        var fields = new List<Field>
        {
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].ErpSourceId",
                    Value = "Erp Source Id 1"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].Company",
                    Value = "Company Test 1"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].ContactPersons[0].Country",
                    Value = "United States"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].ContactPersons[0].FirstName",
                    Value = "Ryan"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].ContactPersons[0].LastName",
                    Value = "Clark"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].ContactPersons[1].Country",
                    Value = "Mexico"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].ContactPersons[1].FirstName",
                    Value = "Miguel"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].ContactPersons[1].LastName",
                    Value = "Martinez"
                }
            }
        };

        // Action
        var expectedResult = ConverterObjectHelper.CheckOverAllProperties(
            fields,
            systemObject,
            true);
        
        // Assertion
        Assert.Single(expectedResult);
        Assert.Equal(expectedResult[0].ErpSourceId, "Erp Source Id 1");
        Assert.Equal(expectedResult[0].Company, "Company Test 1");
        Assert.Equal(expectedResult[0].ContactPersons[0].Country, "United States");
        Assert.Equal(expectedResult[0].ContactPersons[0].FirstName, "Ryan");
        Assert.Equal(expectedResult[0].ContactPersons[0].LastName, "Clark");
        Assert.Equal(expectedResult[0].ContactPersons[1].Country, "Mexico");
        Assert.Equal(expectedResult[0].ContactPersons[1].FirstName, "Miguel");
        Assert.Equal(expectedResult[0].ContactPersons[1].LastName, "Martinez");
    }
    [Fact]
    public void GetElementsFromFields_SetFieldsIndexZeroAndAsList_ReturnFieldsRemovingListIndicator()
    {
        // Arrange
        var systemObject = "SupplierConfigurationImportBatches";
        var fields = new List<Field>
        {
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].ErpSourceId",
                    Value = "Erp Source Id 1"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].Company",
                    Value = "Company Test 1"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].ContactPersons[0].Country",
                    Value = "United States"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].ContactPersons[0].FirstName",
                    Value = "Ryan"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].ContactPersons[0].LastName",
                    Value = "Clark"
                }
            }
        };

        // Action
        var currentResult = ConverterObjectHelper.GetElementsFromFields(
            fields,
            systemObject,
            0,
            true);
        
        // Assertion
        Assert.NotEmpty(currentResult);
        Assert.Equal("ErpSourceId", currentResult.ElementAt(0).Name);
        Assert.Equal("Company", currentResult.ElementAt(1).Name);
        Assert.Equal(
            "ContactPersons[0].Country",
            currentResult.ElementAt(2).Name);
    }

}