using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FitnessTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddExerciseTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExerciseTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    MuscleGroup = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseTemplates", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ExerciseTemplates",
                columns: new[] { "Id", "DisplayOrder", "MuscleGroup", "Name" },
                values: new object[,]
                {
                    { 1, 1, 0, "Incline Dumbbell Press" },
                    { 2, 2, 0, "Flat Bench Press" },
                    { 3, 3, 0, "Seated Butterfly" },
                    { 4, 4, 0, "Push-ups" },
                    { 5, 1, 1, "Tricep Pushdown" },
                    { 6, 2, 1, "Overhead Dumbbell Extension" },
                    { 7, 3, 1, "Single Arm Cable Pushdown" },
                    { 8, 4, 1, "Dips" },
                    { 9, 1, 2, "Lat Pulldown" },
                    { 10, 2, 2, "Seated Row" },
                    { 11, 3, 2, "One Arm Dumbbell Row" },
                    { 12, 4, 2, "T-Bar Row" },
                    { 13, 1, 3, "Rear Delt Fly" },
                    { 14, 2, 3, "Lateral Raises" },
                    { 15, 3, 3, "Seated Dumbbell Shoulder Press" },
                    { 16, 4, 3, "Shrugs" },
                    { 17, 1, 4, "Barbell Curl" },
                    { 18, 2, 4, "Preacher Curl" },
                    { 19, 3, 4, "Cable Curl" },
                    { 20, 4, 4, "Hammer Curl" },
                    { 21, 1, 5, "Leg Press" },
                    { 22, 2, 5, "Leg Extension" },
                    { 23, 3, 5, "Hamstring Curl" },
                    { 24, 4, 5, "Calf Raises" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseTemplates");
        }
    }
}
