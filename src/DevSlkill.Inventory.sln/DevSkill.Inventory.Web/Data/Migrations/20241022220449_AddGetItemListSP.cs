using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGetItemListSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                CREATE OR ALTER PROCEDURE GetItemList
                	@PageIndex int,
                	@PageSize int,
                	@OrderBy nvarchar(100),
                	@ItemName nvarchar(100) = '%',
                	@Barcode nvarchar(100) = '%',
                	@PriceFrom decimal = NULL,
                	@PriceTo decimal = NULL,
                	@CategoryId uniqueIdentifier = NULL,
                	@WarehouseId uniqueIdentifier = NULL,
                    @IsActive BIT = 0,
                    @BelowMinimumStock BIT = 0,
                	@Total int output,
                	@TotalDisplay int output
                AS
                BEGIN
                	DECLARE @CountSql NVARCHAR(MAX);
                	DECLARE @CountParamList NVARCHAR(MAX);

                	--Get Total Count
                	SELECT @Total = COUNT(*) FROM Items;

                	--Get Total Display Count
                	SELECT @countsql = 'SELECT @TotalDisplay = COUNT(DISTINCT I.Id) FROM Items I
                						INNER JOIN MeasurementUnits MU on I.MeasurementUnitId = MU.Id
                						LEFT JOIN ItemCategories IC on I.CategoryId = IC.Id
                						LEFT JOIN Taxes T on I.TaxId = T.Id
                						LEFT JOIN ItemWarehouses IW on I.Id = IW.ItemId
                						LEFT JOIN Warehouses W on IW.WarehouseId = W.Id
                						WHERE 1 = 1 ';

                	IF @ItemName IS NOT NULL AND @ItemName != '%'
                	SET @CountSql = @CountSql + ' AND i.ItemName LIKE ''%'' + @xItemName + ''%''';

                	IF @Barcode IS NOT NULL AND @Barcode != '%'
                	SET @CountSql = @CountSql + ' AND i.Barcode LIKE ''%'' + @xBarcode + ''%''';

                	IF @CategoryId IS NOT NULL
                	SET @CountSql = @CountSql + ' AND i.CategoryId = @xCategoryId ';

                	IF @WarehouseId IS NOT NULL
                	SET @CountSql = @CountSql + ' AND iw.WarehouseId = @xWarehouseId ';

                	IF @BelowMinimumStock = 1
                	SET @CountSql = @CountSql + ' AND IW.StockQuantity < I.MinimumStockQuantity ';

                	IF @IsActive = 1
                	SET @CountSql = @CountSql + ' AND I.IsActive = 1 ';

                	IF @PriceFrom IS NOT NULL
                	SET @CountSql = @CountSql + '  AND I.SellingPrice >= @xPriceFrom ';

                	IF @PriceTo IS NOT NULL
                	SET @CountSql = @CountSql + '  AND I.SellingPrice <= @xPriceTo ';

                	SET @CountParamList = '
                		@xItemName nvarchar(100),
                		@xBarcode nvarchar(100),
                		@xCategoryId uniqueIdentifier,
                		@xWarehouseId uniqueIdentifier,
                		@xPriceFrom decimal,
                		@xPriceTo decimal,
                		@TotalDisplay int output' ;

                	EXECUTE SP_EXECUTESQL @CountSql, @CountParamList,
                						@ItemName,
                						@Barcode,
                						@CategoryId,
                						@WarehouseId,
                						@PriceFrom,
                						@PriceTo,
                						@TotalDisplay = @TotalDisplay output;
                ----------------------------------------------------------------------------------

                	--Get Data
                	DECLARE @Sql NVARCHAR(MAX);
                	DECLARE @ParamList NVARCHAR(MAX);

                	SELECT @Sql = 'Select 
                					I.Id,
                					I.Picture,
                					I.ItemName,
                					I.Barcode,
                					IC.Name as Category,
                					I.SellingPrice As Price,
                					T.Parcentage As Tax,
                					I.MinimumStockQuantity,
                					SUM(StockQuantity) AS TotalStockQuantity, 
                					MU.Symbol as MeasurementUnit,
                					I.IsActive

                					FROM Items I
                					INNER JOIN MeasurementUnits MU on I.MeasurementUnitId = MU.Id
                					LEFT JOIN ItemCategories IC on I.CategoryId = IC.Id
                					LEFT JOIN Taxes T on I.TaxId = T.Id
                					LEFT JOIN ItemWarehouses IW on I.Id = IW.ItemId
                					LEFT JOIN Warehouses W on IW.WarehouseId = W.Id
                					WHERE 1 = 1 ';


                	IF @ItemName IS NOT NULL AND @ItemName != '%'
                	SET @Sql = @Sql + ' AND i.ItemName LIKE ''%'' + @xItemName + ''%''';

                	IF @Barcode IS NOT NULL AND @Barcode != '%'
                	SET @Sql = @Sql + ' AND i.Barcode LIKE ''%'' + @xBarcode + ''%''';

                	IF @CategoryId IS NOT NULL
                	SET @Sql = @Sql + ' AND i.CategoryId = @xCategoryId ';

                	IF @WarehouseId IS NOT NULL
                	SET @Sql = @Sql + ' AND iw.WarehouseId = @xWarehouseId ';

                	IF @BelowMinimumStock = 1
                	SET @Sql = @Sql + ' AND IW.StockQuantity < I.MinimumStockQuantity ';

                	IF @IsActive = 1
                	SET @Sql = @Sql + ' AND I.IsActive = 1 ';

                	IF @PriceFrom IS NOT NULL
                	SET @Sql = @Sql + '  AND I.SellingPrice >= @xPriceFrom ';

                	IF @PriceTo IS NOT NULL
                	SET @Sql = @Sql + '  AND I.SellingPrice <= @xPriceTo ';

                	SET @Sql = @Sql + ' GROUP BY
                						I.Id,
                						I.Picture, 
                						I.ItemName, 
                						I.Barcode, 
                						IC.Name, 
                						I.SellingPrice, 
                						T.Parcentage,
                						I.MinimumStockQuantity,
                						MU.Symbol, 
                						I.IsActive ';

                	SET @Sql = @Sql + ' Order by '+@OrderBy+' OFFSET @PageSize * (@PageIndex - 1) 
                						ROWS FETCH NEXT @PageSize ROWS ONLY';

                	SET @ParamList = '
                		@xItemName nvarchar(100),
                		@xBarcode nvarchar(100),
                		@xCategoryId uniqueIdentifier,
                		@xWarehouseId uniqueIdentifier,
                		@xPriceFrom decimal,
                		@xPriceTo decimal,
                		@PageIndex int,
                		@PageSize int';

                	EXECUTE SP_EXECUTESQL @Sql, @ParamList,
                							@ItemName,
                							@Barcode,
                							@CategoryId,
                							@WarehouseId,
                							@PriceFrom,
                							@PriceTo,
                							@PageIndex,
                							@PageSize;

                	Print @CountSql;
                	PRINT @Sql;
                END
                GO
                """;

            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE GetItemList");
        }
    }
}
