using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace campus.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    isTeacher = table.Column<bool>(type: "boolean", nullable: false),
                    isAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    isStudent = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    StartYear = table.Column<int>(type: "integer", nullable: false),
                    MaximumStudentsCount = table.Column<int>(type: "integer", nullable: false),
                    RemainingSlotsCount = table.Column<int>(type: "integer", nullable: false),
                    Semester = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Requirements = table.Column<string>(type: "text", nullable: false),
                    Annotations = table.Column<string>(type: "text", nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    IsImportant = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    MidtermResult = table.Column<int>(type: "integer", nullable: false),
                    FinalResult = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => new { x.AccountId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_Students_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsMain = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => new { x.AccountId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_Teachers_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Teachers_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "BirthDate", "CreatedDate", "Email", "FullName", "Password", "isAdmin", "isStudent", "isTeacher" },
                values: new object[,]
                {
                    { new Guid("0572342d-fde4-48f6-a754-350a15edbcc0"), new DateTime(2005, 6, 17, 14, 53, 29, 390, DateTimeKind.Utc), new DateTime(2024, 11, 24, 18, 12, 4, 390, DateTimeKind.Utc), "yanepashka@example.com", "Пашка", "$2a$11$LpRIy5FmkONerK2u6A.sxOe2/jdhrIp5R6g6tCe.PYc0TVcZ8P6S2", false, true, false },
                    { new Guid("31f3d270-9cdc-46e4-a807-0a672ad72a6b"), new DateTime(2005, 11, 23, 14, 53, 29, 390, DateTimeKind.Utc), new DateTime(2024, 11, 24, 18, 12, 1, 390, DateTimeKind.Utc), "dangerlyonya@example.com", "Дэнжер Лёня", "$2a$11$tzSMItiH0d5P3hdV5.JJ5eQqHeBAgY7SBt5HrKfThy3QprO04Eh5.", false, true, true },
                    { new Guid("a08d8946-8bac-45e2-afe3-a0ce7795008a"), new DateTime(2004, 11, 23, 14, 53, 29, 390, DateTimeKind.Utc), new DateTime(2024, 11, 24, 18, 12, 0, 390, DateTimeKind.Utc), "sanyasigmagucci@example.com", "Александр Сигмов", "$2a$11$FFJfJPoKTIU/XJBn1bQc5OCnxbV89zkj4Y9c5PnxF20sxObypqmni", true, false, true },
                    { new Guid("cddc13d6-ce4a-472e-87ca-24b8a10f88e8"), new DateTime(1995, 11, 23, 14, 53, 29, 390, DateTimeKind.Utc), new DateTime(2024, 11, 24, 18, 12, 2, 390, DateTimeKind.Utc), "lovebackend@example.com", "Антонио Бекэндрос", "$2a$11$acdEdyPyO584yYRhZtnhCe.CtVyu0Ne2FdWeExLMfrEp/y3pAUE7i", false, false, true },
                    { new Guid("f0ed1390-59f0-4e80-8559-3a7850d47a2f"), new DateTime(2006, 10, 1, 14, 53, 29, 390, DateTimeKind.Utc), new DateTime(2024, 11, 24, 18, 12, 3, 390, DateTimeKind.Utc), "beerlesha@example.com", "Леша Подпивнов 228", "$2a$11$Ihv6krc.4HJ.5by1hC2qOe7KG7NP8Qw3n3lgvMEOw7MIpK4qaUHTC", false, true, false }
                });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "Id", "CreatedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("4fe8a005-f169-45f8-adbd-1e5fe1d7af50"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Backend" },
                    { new Guid("7a4ca155-ada6-47e4-b9d0-f2d4b6467039"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Frontend" }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Annotations", "CreatedDate", "GroupId", "MaximumStudentsCount", "Name", "RemainingSlotsCount", "Requirements", "Semester", "StartYear", "Status" },
                values: new object[,]
                {
                    { new Guid("4288c2aa-c310-41a3-b2d8-04a8a970504d"), "Будем кнопки красить", new DateTime(2023, 11, 24, 18, 12, 12, 390, DateTimeKind.Utc), new Guid("7a4ca155-ada6-47e4-b9d0-f2d4b6467039"), 2, "React", 0, "Красивые отступы", 0, 2023, 3 },
                    { new Guid("cce72df6-7f8f-4fea-a36a-25875849c1a6"), "Какая-то аннотация", new DateTime(2023, 11, 24, 18, 12, 11, 390, DateTimeKind.Utc), new Guid("4fe8a005-f169-45f8-adbd-1e5fe1d7af50"), 5, "PHP", 4, "Какие-то требования", 0, 2024, 2 },
                    { new Guid("d5c75cec-b93e-42fb-9725-7f4aaf57dd28"), "Энтити фрамерворк рулит", new DateTime(2023, 11, 24, 18, 12, 10, 390, DateTimeKind.Utc), new Guid("4fe8a005-f169-45f8-adbd-1e5fe1d7af50"), 10, "ASPNET", 9, "Сдать хотя бы с 4го раза", 1, 2025, 1 }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "CourseId", "CreatedDate", "IsImportant", "Text" },
                values: new object[,]
                {
                    { new Guid("10d28aa7-7e7f-419c-84c2-ea8601233094"), new Guid("d5c75cec-b93e-42fb-9725-7f4aaf57dd28"), new DateTime(2024, 11, 24, 18, 13, 14, 390, DateTimeKind.Utc), false, "У него спрашивают, а что не в в машинке не стираешь?" },
                    { new Guid("1be6757b-5096-4a73-a8b1-5552b6c09931"), new Guid("d5c75cec-b93e-42fb-9725-7f4aaf57dd28"), new DateTime(2024, 11, 24, 18, 13, 13, 390, DateTimeKind.Utc), false, "Он отвечает, а я ее стираю" },
                    { new Guid("3ef63582-42af-41a8-bfb9-268681f24934"), new Guid("d5c75cec-b93e-42fb-9725-7f4aaf57dd28"), new DateTime(2024, 11, 24, 18, 13, 12, 390, DateTimeKind.Utc), false, "Ему говорят, мол мужик что ты в одежде купаешься?" },
                    { new Guid("53e410fe-df1d-423b-86c5-84a75e113506"), new Guid("d5c75cec-b93e-42fb-9725-7f4aaf57dd28"), new DateTime(2024, 11, 24, 18, 13, 10, 390, DateTimeKind.Utc), true, "Анекдот" },
                    { new Guid("5b88985c-c0c2-4820-87cc-d7f954fdbfd1"), new Guid("d5c75cec-b93e-42fb-9725-7f4aaf57dd28"), new DateTime(2024, 11, 24, 18, 13, 11, 390, DateTimeKind.Utc), false, "Купается одетый мужик в речке" },
                    { new Guid("ab00d7de-8c63-487d-a1da-417e6b994c0f"), new Guid("d5c75cec-b93e-42fb-9725-7f4aaf57dd28"), new DateTime(2024, 11, 24, 18, 13, 15, 390, DateTimeKind.Utc), false, "А он говорит: меня в ней укачивает" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "AccountId", "CourseId", "FinalResult", "MidtermResult", "Status" },
                values: new object[,]
                {
                    { new Guid("0572342d-fde4-48f6-a754-350a15edbcc0"), new Guid("cce72df6-7f8f-4fea-a36a-25875849c1a6"), 0, 0, 1 },
                    { new Guid("0572342d-fde4-48f6-a754-350a15edbcc0"), new Guid("d5c75cec-b93e-42fb-9725-7f4aaf57dd28"), 0, 0, 0 },
                    { new Guid("31f3d270-9cdc-46e4-a807-0a672ad72a6b"), new Guid("4288c2aa-c310-41a3-b2d8-04a8a970504d"), 1, 1, 1 },
                    { new Guid("f0ed1390-59f0-4e80-8559-3a7850d47a2f"), new Guid("4288c2aa-c310-41a3-b2d8-04a8a970504d"), 2, 2, 1 },
                    { new Guid("f0ed1390-59f0-4e80-8559-3a7850d47a2f"), new Guid("cce72df6-7f8f-4fea-a36a-25875849c1a6"), 0, 0, 2 },
                    { new Guid("f0ed1390-59f0-4e80-8559-3a7850d47a2f"), new Guid("d5c75cec-b93e-42fb-9725-7f4aaf57dd28"), 0, 0, 1 }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "AccountId", "CourseId", "IsMain" },
                values: new object[,]
                {
                    { new Guid("31f3d270-9cdc-46e4-a807-0a672ad72a6b"), new Guid("d5c75cec-b93e-42fb-9725-7f4aaf57dd28"), false },
                    { new Guid("a08d8946-8bac-45e2-afe3-a0ce7795008a"), new Guid("4288c2aa-c310-41a3-b2d8-04a8a970504d"), true },
                    { new Guid("cddc13d6-ce4a-472e-87ca-24b8a10f88e8"), new Guid("cce72df6-7f8f-4fea-a36a-25875849c1a6"), true },
                    { new Guid("cddc13d6-ce4a-472e-87ca-24b8a10f88e8"), new Guid("d5c75cec-b93e-42fb-9725-7f4aaf57dd28"), true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Email",
                table: "Accounts",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_GroupId",
                table: "Courses",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Id",
                table: "Courses",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_Id",
                table: "Groups",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CourseId",
                table: "Notifications",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_CourseId",
                table: "Students",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_CourseId",
                table: "Teachers",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Groups");
        }
    }
}
