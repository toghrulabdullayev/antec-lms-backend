# Contributing to AntecLMS

Welcome! This guide walks you through the full contribution workflow step by step. Follow it every time you make a change.

---

## 1. Start from `main` and get the latest changes

Before creating a new branch, make sure you are on `main` and it is up to date.

```bash
# Switch to main
git checkout main

# Pull the latest changes from the remote
git pull origin main
```

> **Why?** If you start from an outdated `main`, you may run into merge conflicts later.

---

## 2. Create a new branch for your work

Name your branch using the format: `type/short-description`

Examples:
- `feature/add-login-page`
- `fix/user-validation-error`
- `chore/update-dependencies`

```bash
git checkout -b feature/your-feature-name
```

> **Tip:** Never commit directly to `main`. Always use a feature branch.

---

## 3. Double-check you are on the correct branch

Before writing any code, verify your current branch:

```bash
git branch
```

The active branch is highlighted with a `*`. Make sure it is **not** `main` — it should be your feature branch.

Run this command anytime you are unsure.

---

## 4. Make your changes

Write your code.

> **Tip:** Use `git status` to see what files were changed.

---

## 5. Create a new migration if you changed entities

If you modified, added, or removed any entity classes (e.g. `User`, `Course`, `Lesson`), you must create a new EF Core migration and apply it.

Run these commands from the **project root**:

```bash
dotnet ef migrations add DescribeYourChanges \
  --project src/AntecLMS.Infrastructure \
  --startup-project src/AntecLMS.API

dotnet ef database update \
  --project src/AntecLMS.Infrastructure \
  --startup-project src/AntecLMS.API
```

Replace `DescribeYourChanges` with a short name, e.g. `AddUserAvatarUrl`.

> **Why?** Migrations keep the database schema in sync with your code. Running `database update` locally also verifies the migration works before you commit.

---

## 6. Format your code with CSharpier

Run the formatter from the **project root** (the folder containing `AntecLMS.slnx`):

```bash
dotnet csharpier format .
```

This ensures all code follows the same style.

> **Why?** Consistent formatting keeps pull requests clean and code reviews focused on logic, not style.

---

## 7. Build the project to verify everything compiles

```bash
dotnet build
```

If the build fails, fix the errors before committing. Do not commit broken code.

> **Why?** A green build means your changes are safe to merge.

---

## 8. Stage everything

```bash
git add .
```

---

## 9. Commit your changes

Write a clear, concise commit message:

```bash
git commit -m "type: short description"
```

Examples:
- `feat: add login page`
- `fix: handle empty email on user registration`
- `chore: update CSharpier config`

> **Convention:** We use [Conventional Commits](https://www.conventionalcommits.org/). The most common types are `feat`, `fix`, `chore`, `refactor`, `docs`, `test`.

---

## 10. Push your branch to the remote

```bash
git push origin feature/your-feature-name
```

---

## 11. Communicate with the team before opening a PR

Before you open a pull request, let the team know what you are about to do. This helps avoid situations where two people edit the same file at the same time.

A quick message in the team chat works:

> "About to open a PR for login page — anyone working on user-related files?"

If someone is already in the same area, coordinate who does what or wait until they merge first.

> **Why?** A little communication upfront saves everyone from messy merge conflicts.

---

## 12. Open a Pull Request

1. Go to the repository on GitHub.
2. You will see a banner suggesting your recently pushed branch — click **Compare & pull request**.
3. Set the title to match your commit message (e.g. `feat: add login page`).
4. Add a short description of what the PR does and why.
5. Click **Create pull request**.

> **Important:** After opening the PR, wait for the reviewer (me) to approve it. Do not merge yourself.

---

## 13. After the PR is approved and merged

Once the PR is closed (merged), switch back to `main` and pull the latest changes:

```bash
git checkout main
git pull origin main
```

Now you are ready to start the next task from step 1.

---

## Keeping your branch in sync

Whenever someone else has merged changes, pull the latest `main` into your current branch to stay up to date:

```bash
git pull origin main
```

Run this whether you are on a feature branch or on `main` — it ensures you always work against the latest code.

---

## Quick reference

```bash
# Start fresh
git checkout main
git pull origin main
git checkout -b feature/my-task

# Do work, then:
dotnet ef migrations add MyChange \
  --project src/AntecLMS.Infrastructure \
  --startup-project src/AntecLMS.API
dotnet ef database update \
  --project src/AntecLMS.Infrastructure \
  --startup-project src/AntecLMS.API
dotnet csharpier format .
dotnet build
git add .
git commit -m "feat: my task"
git push origin feature/my-task

# Open PR on GitHub, wait for approval, then:
git checkout main
git pull origin main
```

---

## Need help?

If you are unsure about any step, ask before proceeding. It is better to ask than to break something.
