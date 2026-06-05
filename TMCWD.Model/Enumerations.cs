public enum UserRole
{
    SuperAdmin = 1,
    CustomerRepresentative = 2,
    Engineer = 3,
    Guest = 4
}

public enum ErrorType
{
    Error = 1,
    Warning = 2
}

public enum ErrorModule
{
    Administration = 1,
    Application = 2,
    CustomerSupport = 3,
    Data = 4,
    Engineering = 5
}

public enum RequestStatus
{
    Draft = 1,
    InProgress = 2,
    Completed = 3,
    Rejected = 4
}

public enum AccountStatus
{
    Pending = 1,
    Suspended = 2,
    Active = 3,
    Closed = 4
}

public enum JobOrderStatus
{
    Inspection = 1,
    Charging = 2,
    Payment = 3,
    Releasing = 4,
    Installation = 5,
    Verification = 6,
    Completed = 7,
    Rejected = 8
}

public enum FileType
{
    Finding = 1,
    Verification = 2
}