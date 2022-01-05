
## Task

### Add Migration

```bash
$ cd src/Sample.Data
$ dotnet ef migrations add "Initialize database" --context AppDbContext --startup-project ../Sample.App/ --project ../Sample.Data.SqlServer/ --json
```