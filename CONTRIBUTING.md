# 👥 Contributing to `akordai/net-sdk`

Thanks for your interest in contributing to [Neuroglia A2A](https://github.com/neuroglia-io/a2a).

Whether you're fixing bugs, improving documentation, or implementing new capabilities — we appreciate your help!

---

## 🚀 Getting Started

1. **Fork the repository**
2. **Clone your fork**  
   ```bash
   git clone https://github.com/YOUR-USERNAME/net-sdk
   cd net-sdk
   ```

3. **Set up dependencies**  
   ```bash
   dotnet restore
   ```

4. **Build the solution**  
   ```bash
   dotnet build
   ```

5. **Run tests**  
   ```bash
   dotnet test
   ```

---

## 🧠 Guidelines

### ✅ Do

- Use clear, self-explanatory commit messages
- Follow C# conventions
- Add or update unit/integration tests when applicable
- Write or improve XML `<summary>` docs for public types and members

### ❌ Don’t

- Introduce breaking changes without discussion
- Leave `TODO` comments in committed code
- Reformat unrelated files in the same PR

---

## 💡 Feature Contributions

1. Open an issue or discussion to validate your idea
2. Fork and branch from `main`
3. Create a descriptive PR — include context, screenshots, YAML examples if relevant

---

## 🧪 Testing

All PRs must pass unit tests and schema validation. We're actively adding more test coverage — feel free to contribute!

---

## 📚 Documentation

Public APIs, YAML schemas, and DSL syntax should be documented in:

- XML comments (`<summary>`, etc.)
- Markdown files in `/docs`
- Example YAML snippets under `/samples`

---

## 🛡️ Code of Conduct

We are committed to a respectful and inclusive environment. Please read our [Code of Conduct](./CODE_OF_CONDUCT.md) before contributing.

---

## 🙏 Thank You

You're helping shape a modular, open foundation for agent orchestration in AI-native applications. We’re grateful you're here.

— The Neuroglia Team
