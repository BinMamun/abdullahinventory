using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class GetStockListSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                CREATE OR ALTER PROCEDURE GetStockList
                	@PageIndex int,
                	@PageSize int,
                	@OrderBy nvarchar(100),
                	@ItemName nvarchar(100) = '%',
                	@Barcode nvarchar(100) = '%',
                	@CategoryId uniqueIdentifier = NULL,
                	@WarehouseId uniqueIdentifier = NULL,
                    @StockGreaterThanZero BIT = 0,
                    @BelowMinimumStock BIT = 0,
                	@Total int output,
                	@TotalDisplay int output
                AS
                BEGIN

                	DECLARE @CountSql NVARCHAR(2000);
                	DECLARE @CountParamList NVARCHAR(MAX);

                	--Get Total Count
                	SELECT @Total = COUNT(*) FROM Warehouses w
                							 INNER JOIN ItemWarehouses iw ON w.Id = iw.WarehouseId
                							 INNER JOIN Items i ON iw.ItemId = i.Id
                							 LEFT JOIN ItemCategories ic ON i.CategoryId = ic.Id
                							 INNER JOIN MeasurementUnits mu ON i.MeasurementUnitId = mu.Id;

                	--Get Total Display Count
                	SELECT @countsql = 'SELECT @TotalDisplay = COUNT(*) FROM Warehouses w
                						INNER JOIN ItemWarehouses iw ON w.Id = iw.WarehouseId
                						INNER JOIN Items i ON iw.ItemId = i.Id
                						LEFT JOIN ItemCategories ic ON i.CategoryId = ic.Id
                						INNER JOIN MeasurementUnits mu ON i.MeasurementUnitId = mu.Id
                						WHERE 1 = 1 ';

                	IF @ItemName IS NOT NULL AND @ItemName != '%'
                	SET @CountSql = @CountSql + ' AND i.ItemName LIKE ''%'' + @xItemName + ''%'''

                	IF @Barcode IS NOT NULL AND @Barcode != '%'
                	SET @CountSql = @CountSql + ' AND i.Barcode LIKE ''%'' + @xBarcode + ''%'''

                	IF @CategoryId IS NOT NULL
                	SET @CountSql = @CountSql + ' AND i.CategoryId = @xCategoryId '

                	IF @WarehouseId IS NOT NULL
                	SET @CountSql = @CountSql + ' AND iw.WarehouseId = @xWarehouseId '

                	IF @StockGreaterThanZero = 1
                	SET @CountSql = @CountSql + ' AND IW.StockQuantity > 0 ';

                	IF @BelowMinimumStock = 1
                	SET @CountSql = @CountSql + ' AND IW.StockQuantity < I.MinimumStockQuantity ';

                	SET @CountParamList = '
                		@xItemName nvarchar(100),
                		@xBarcode nvarchar(100),
                		@xCategoryId uniqueIdentifier,
                		@xWarehouseId uniqueIdentifier,
                		@TotalDisplay int output' ;


                	EXECUTE SP_EXECUTESQL @CountSql, @CountParamList,
                							@ItemName,
                							@Barcode,
                							@CategoryId,
                							@WarehouseId,
                							@TotalDisplay = @TotalDisplay output;
                ----------------------------------------------------------------------------------

                	--Get Data
                	DECLARE @Sql NVARCHAR(2000);
                	DECLARE @ParamList NVARCHAR(MAX);

                	SELECT @Sql = 'SELECT 
                					I.ItemName,
                					I.Barcode,
                					IC.[Name] as CategoryName,
                					W.[Name] as WarehouseName, 
                					IW.StockQuantity, 
                					I.MinimumStockQuantity,
                					MU.Symbol, 
                					IW.CostPerUnit

                					FROM Warehouses W
                					INNER JOIN ItemWarehouses IW ON W.Id = IW.WarehouseId
                					INNER JOIN Items I ON IW.ItemId = I.Id
                					LEFT JOIN ItemCategories ic ON i.CategoryId = ic.Id
                					INNER JOIN MeasurementUnits MU ON I.MeasurementUnitId = MU.Id 
                					WHERE 1 = 1 ';

                	IF @ItemName IS NOT NULL AND @ItemName != '%'
                	SET @Sql = @Sql + ' AND i.ItemName LIKE ''%'' + @xItemName + ''%'''

                	IF @Barcode IS NOT NULL AND @Barcode != '%'
                	SET @Sql = @Sql + ' AND i.Barcode LIKE ''%'' + @xBarcode + ''%'''

                	IF @CategoryId IS NOT NULL
                	SET @Sql = @Sql + ' AND i.CategoryId = @xCategoryId '

                	IF @WarehouseId IS NOT NULL
                	SET @Sql = @Sql + ' AND iw.WarehouseId = @xWarehouseId '

                	IF @StockGreaterThanZero = 1
                	SET @SQL = @SQL + ' AND IW.StockQuantity > 0 ';

                	IF @BelowMinimumStock = 1
                	SET @SQL = @SQL + ' AND IW.StockQuantity < I.MinimumStockQuantity ';

                	SET @Sql = @Sql + ' Order by '+@OrderBy+' OFFSET @PageSize * (@PageIndex - 1) 
                						ROWS FETCH NEXT @PageSize ROWS ONLY';

                	SET @ParamList = '
                		@xItemName nvarchar(100),
                		@xBarcode nvarchar(100),
                		@xCategoryId uniqueIdentifier,
                		@xWarehouseId uniqueIdentifier,
                		@PageIndex int,
                		@PageSize int';

                	EXECUTE SP_EXECUTESQL @Sql, @ParamList,
                							@ItemName,
                							@Barcode,
                							@CategoryId,
                							@WarehouseId,
                							@PageIndex,
                							@PageSize;
                END
                GO
                """;

            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE GetStockList");
        }
    }
}
