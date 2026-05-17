namespace Lab06;

/// <summary>
/// Дискреційна політика безпеки кондитерського підприємства «Солодкий Світ».
/// </summary>
public static class SecurityPolicy
{
    public const int RightRead = 1;
    public const int RightEdit = 2;
    public const int RightDelete = 3;
    public const int RightDenied = 4;

    public static readonly string[] SubjectIds = ["S1", "S2", "S3", "S4", "S5"];

    public static readonly string[] ObjectIds = ["O1", "O2", "O3", "O4"];

    public static readonly string[] ObjectNames =
    [
        "Комерційна та конфіденційна інформація",
        "Дані про робітників",
        "Дані про споживачів",
        "Дані про клієнтів"
    ];

    public static readonly string[] RightLabels =
    [
        "1 - читання",
        "2 - редагування",
        "3 - видалення",
        "4 - доступ заборонено"
    ];

    private static readonly int[,] AccessMatrix =
    {
        { 2, 2, 2, 2 },
        { 4, 1, 1, 1 },
        { 4, 1, 1, 1 },
        { 4, 1, 2, 2 },
        { 4, 3, 3, 3 }
    };

    private static readonly Dictionary<string, UserAccount> _accounts = new(StringComparer.OrdinalIgnoreCase);

    public static IReadOnlyDictionary<string, UserAccount> Accounts => _accounts;

    static SecurityPolicy()
    {
        RegisterPreset("director", "111111", 0, "Козлов В.С.", "Директор");
        RegisterPreset("secretar", "222222", 1, "Новіков О.В.", "Секретар");
        RegisterPreset("operator", "333333", 2, "Гордієнко О.В.", "Оператор");
        RegisterPreset("meneger", "444444", 3, "Марченко О.А.", "Менеджер");
        RegisterPreset("admin", "555555", 4, "Лисенко В.І.", "Адміністратор");
    }

    private static void RegisterPreset(string login, string password, int subjectIndex, string fullName, string position)
    {
        _accounts[login] = new UserAccount(login, password, subjectIndex, fullName, position);
    }

    public static bool TryAuthenticate(string login, string password, out UserAccount? account)
    {
        account = null;
        if (!_accounts.TryGetValue(login.Trim(), out var user))
            return false;

        if (!string.Equals(user.Password, password, StringComparison.Ordinal))
            return false;

        account = user;
        return true;
    }

    public static bool RegisterUser(string login, string password, int subjectIndex, string fullName, string position)
    {
        login = login.Trim();
        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            return false;

        if (subjectIndex < 0 || subjectIndex >= SubjectIds.Length)
            return false;

        if (_accounts.ContainsKey(login))
            return false;

        _accounts[login] = new UserAccount(login, password, subjectIndex, fullName.Trim(), position.Trim());
        return true;
    }

    public static int GetAccessRight(int subjectIndex, int objectIndex)
    {
        if (subjectIndex < 0 || subjectIndex >= SubjectIds.Length)
            return RightDenied;

        if (objectIndex < 0 || objectIndex >= ObjectIds.Length)
            return RightDenied;

        return AccessMatrix[subjectIndex, objectIndex];
    }

    public static bool IsAccessAllowed(int subjectIndex, int objectIndex, int requestedRight)
    {
        int granted = GetAccessRight(subjectIndex, objectIndex);
        if (granted == RightDenied)
            return false;

        return granted >= requestedRight;
    }

    public static string DescribeRight(int right) =>
        right switch
        {
            RightRead => "читання",
            RightEdit => "редагування",
            RightDelete => "видалення",
            RightDenied => "доступ заборонено",
            _ => "невідомо"
        };

    public static string FormatMatrix()
    {
        var lines = new List<string>
        {
            $"{"",-12}" + string.Join("", ObjectIds.Select(id => $"{id,6}"))
        };

        for (int s = 0; s < SubjectIds.Length; s++)
        {
            var row = $"{SubjectIds[s],-12}";
            for (int o = 0; o < ObjectIds.Length; o++)
                row += $"{AccessMatrix[s, o],6}";
            lines.Add(row);
        }

        return string.Join(Environment.NewLine, lines);
    }
}

public sealed record UserAccount(
    string Login,
    string Password,
    int SubjectIndex,
    string FullName,
    string Position)
{
    public string SubjectId => SecurityPolicy.SubjectIds[SubjectIndex];
}
