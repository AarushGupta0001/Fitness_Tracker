using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FitnessTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddExerciseObjectsWeightsAndWorkoutTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkoutTypeId",
                table: "WorkoutSessions",
                type: "int",
                nullable: false,
                defaultValue: 4);

            migrationBuilder.AddColumn<int>(
                name: "ExerciseObjectTypeId",
                table: "ExerciseTemplates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "ExerciseLogs",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AddColumn<int>(
                name: "ExerciseTemplateId",
                table: "ExerciseLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSkipped",
                table: "ExerciseLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "WorkoutSessionId",
                table: "ExerciseLogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExerciseObjectTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseObjectTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "weight_bar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weight_bar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "weight_db",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weight_db", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "weight_machine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weight_machine", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ExerciseObjectTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "dumbel" },
                    { 2, "bar" },
                    { 3, "machine" }
                });

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 1,
                column: "ExerciseObjectTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 2,
                column: "ExerciseObjectTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 3,
                column: "ExerciseObjectTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 4,
                column: "ExerciseObjectTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 5,
                column: "ExerciseObjectTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 6,
                column: "ExerciseObjectTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 7,
                column: "ExerciseObjectTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 8,
                column: "ExerciseObjectTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 9,
                column: "ExerciseObjectTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 10,
                column: "ExerciseObjectTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 11,
                column: "ExerciseObjectTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 12,
                column: "ExerciseObjectTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 13,
                column: "ExerciseObjectTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 14,
                column: "ExerciseObjectTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 15,
                column: "ExerciseObjectTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 16,
                column: "ExerciseObjectTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 17,
                column: "ExerciseObjectTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 18,
                column: "ExerciseObjectTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 19,
                column: "ExerciseObjectTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 20,
                column: "ExerciseObjectTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 21,
                column: "ExerciseObjectTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 22,
                column: "ExerciseObjectTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 23,
                column: "ExerciseObjectTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 24,
                column: "ExerciseObjectTypeId",
                value: 2);

            migrationBuilder.InsertData(
                table: "WorkoutTypes",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "push", "Push" },
                    { 2, "pull", "Pull" },
                    { 3, "leg-shoulder", "Leg + Shoulder" },
                    { 4, "upper", "Upper" },
                    { 5, "lower", "Lower" }
                });

            migrationBuilder.InsertData(
                table: "weight_bar",
                columns: new[] { "Id", "DisplayOrder", "Value" },
                values: new object[,]
                {
                    { 1, 1, 5m },
                    { 2, 2, 7.5m },
                    { 3, 3, 10m },
                    { 4, 4, 12.5m },
                    { 5, 5, 15m },
                    { 6, 6, 17.5m },
                    { 7, 7, 20m },
                    { 8, 8, 22.5m },
                    { 9, 9, 25m },
                    { 10, 10, 27.5m },
                    { 11, 11, 30m },
                    { 12, 12, 32.5m },
                    { 13, 13, 35m }
                });

            migrationBuilder.InsertData(
                table: "weight_db",
                columns: new[] { "Id", "DisplayOrder", "Value" },
                values: new object[,]
                {
                    { 1, 1, 2.5m },
                    { 2, 2, 5m },
                    { 3, 3, 7.5m },
                    { 4, 4, 10m },
                    { 5, 5, 12m },
                    { 6, 6, 12.5m },
                    { 7, 7, 14m },
                    { 8, 8, 15m },
                    { 9, 9, 16m },
                    { 10, 10, 17.5m },
                    { 11, 11, 20m },
                    { 12, 12, 24m }
                });

            migrationBuilder.InsertData(
                table: "weight_machine",
                columns: new[] { "Id", "DisplayOrder", "Value" },
                values: new object[,]
                {
                    { 1, 1, 5m },
                    { 2, 2, 10m },
                    { 3, 3, 15m },
                    { 4, 4, 20m },
                    { 5, 5, 25m },
                    { 6, 6, 30m },
                    { 7, 7, 35m },
                    { 8, 8, 40m },
                    { 9, 9, 45m },
                    { 10, 10, 50m },
                    { 11, 11, 55m },
                    { 12, 12, 60m },
                    { 13, 13, 65m },
                    { 14, 14, 70m },
                    { 15, 15, 75m },
                    { 16, 16, 80m },
                    { 17, 17, 85m },
                    { 18, 18, 90m },
                    { 19, 19, 95m },
                    { 20, 20, 100m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSessions_WorkoutTypeId",
                table: "WorkoutSessions",
                column: "WorkoutTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseTemplates_ExerciseObjectTypeId",
                table: "ExerciseTemplates",
                column: "ExerciseObjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseLogs_ExerciseTemplateId",
                table: "ExerciseLogs",
                column: "ExerciseTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseLogs_WorkoutSessionId",
                table: "ExerciseLogs",
                column: "WorkoutSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseObjectTypes_Name",
                table: "ExerciseObjectTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTypes_Code",
                table: "WorkoutTypes",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseLogs_ExerciseTemplates_ExerciseTemplateId",
                table: "ExerciseLogs",
                column: "ExerciseTemplateId",
                principalTable: "ExerciseTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseLogs_WorkoutSessions_WorkoutSessionId",
                table: "ExerciseLogs",
                column: "WorkoutSessionId",
                principalTable: "WorkoutSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseTemplates_ExerciseObjectTypes_ExerciseObjectTypeId",
                table: "ExerciseTemplates",
                column: "ExerciseObjectTypeId",
                principalTable: "ExerciseObjectTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutSessions_WorkoutTypes_WorkoutTypeId",
                table: "WorkoutSessions",
                column: "WorkoutTypeId",
                principalTable: "WorkoutTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseLogs_ExerciseTemplates_ExerciseTemplateId",
                table: "ExerciseLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseLogs_WorkoutSessions_WorkoutSessionId",
                table: "ExerciseLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseTemplates_ExerciseObjectTypes_ExerciseObjectTypeId",
                table: "ExerciseTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutSessions_WorkoutTypes_WorkoutTypeId",
                table: "WorkoutSessions");

            migrationBuilder.DropTable(
                name: "ExerciseObjectTypes");

            migrationBuilder.DropTable(
                name: "weight_bar");

            migrationBuilder.DropTable(
                name: "weight_db");

            migrationBuilder.DropTable(
                name: "weight_machine");

            migrationBuilder.DropTable(
                name: "WorkoutTypes");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutSessions_WorkoutTypeId",
                table: "WorkoutSessions");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseTemplates_ExerciseObjectTypeId",
                table: "ExerciseTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseLogs_ExerciseTemplateId",
                table: "ExerciseLogs");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseLogs_WorkoutSessionId",
                table: "ExerciseLogs");

            migrationBuilder.DropColumn(
                name: "WorkoutTypeId",
                table: "WorkoutSessions");

            migrationBuilder.DropColumn(
                name: "ExerciseObjectTypeId",
                table: "ExerciseTemplates");

            migrationBuilder.DropColumn(
                name: "ExerciseTemplateId",
                table: "ExerciseLogs");

            migrationBuilder.DropColumn(
                name: "IsSkipped",
                table: "ExerciseLogs");

            migrationBuilder.DropColumn(
                name: "WorkoutSessionId",
                table: "ExerciseLogs");

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "ExerciseLogs",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2,
                oldNullable: true);
        }
    }
}
