using Microsoft.AspNetCore.Identity;

namespace Identidade.API.Extensions;

public class IdentityMensagensPortugues : IdentityErrorDescriber
{
    public override IdentityError DefaultError() => new()
    {
        Code = nameof(DefaultError),
        Description = "Ocorreu um erro desconhecido."
    };

    public override IdentityError ConcurrencyFailure() => new()
    {
        Code = nameof(ConcurrencyFailure),
        Description = "Falha de concorrência otimista, o objeto foi modificado."
    };

    public override IdentityError PasswordMismatch() => new()
    {
        Code = nameof(PasswordMismatch),
        Description = "Senha incorreta."
    };

    public override IdentityError InvalidToken() => new()
    {
        Code = nameof(InvalidToken),
        Description = "Token inválido."
    };

    public override IdentityError LoginAlreadyAssociated() => new()
    {
        Code = nameof(LoginAlreadyAssociated),
        Description = "Um usuário com este login já existe."
    };

    public override IdentityError InvalidUserName(string userName) => new()
    {
        Code = nameof(InvalidUserName),
        Description = $"Nome de usuário '{userName}' inválido, somente letras ou dígitos são permitidos."
    };

    public override IdentityError InvalidEmail(string email) => new()
    {
        Code = nameof(InvalidEmail),
        Description = $"Email '{email}' inválido."
    };

    public override IdentityError DuplicateUserName(string userName) => new()
    {
        Code = nameof(DuplicateUserName),
        Description = $"Nome de usuário '{userName}' já está em uso."
    };

    public override IdentityError DuplicateEmail(string email) => new()
    {
        Code = nameof(DuplicateEmail),
        Description = $"Email '{email}' já está em uso."
    };

    public override IdentityError InvalidRoleName(string role) => new()
    {
        Code = nameof(InvalidRoleName),
        Description = $"Nome de função '{role}' inválido."
    };

    public override IdentityError DuplicateRoleName(string role) => new()
    {
        Code = nameof(DuplicateRoleName),
        Description = $"Função '{role}' já está em uso."
    };

    public override IdentityError UserAlreadyHasPassword() => new()
    {
        Code = nameof(UserAlreadyHasPassword),
        Description = "O usuário já possui uma senha."
    };

    public override IdentityError UserLockoutNotEnabled() => new()
    {
        Code = nameof(UserLockoutNotEnabled),
        Description = "Bloqueio de usuário não habilitado."
    };

    public override IdentityError UserAlreadyInRole(string role) => new()
    {
        Code = nameof(UserAlreadyInRole),
        Description = $"Usuário já está na função '{role}'."
    };

    public override IdentityError UserNotInRole(string role) => new()
    {
        Code = nameof(UserNotInRole),
        Description = $"Usuário não está na função '{role}'."
    };

    public override IdentityError PasswordTooShort(int length) => new()
    {
        Code = nameof(PasswordTooShort),
        Description = $"A senha deve ter pelo menos {length} caracteres."
    };

    public override IdentityError PasswordRequiresNonAlphanumeric() => new()
    {
        Code = nameof(PasswordRequiresNonAlphanumeric),
        Description = "A senha deve ter pelo menos um caractere não alfanumérico."
    };

    public override IdentityError PasswordRequiresDigit() => new()
    {
        Code = nameof(PasswordRequiresDigit),
        Description = "A senha deve ter pelo menos um dígito ('0'-'9')."
    };

    public override IdentityError PasswordRequiresLower() => new()
    {
        Code = nameof(PasswordRequiresLower),
        Description = "A senha deve ter pelo menos uma letra minúscula ('a'-'z')."
    };

    public override IdentityError PasswordRequiresUpper() => new()
    {
        Code = nameof(PasswordRequiresUpper),
        Description = "A senha deve ter pelo menos uma letra maiúscula ('A'-'Z')."
    };

    public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) => new()
    {
        Code = nameof(PasswordRequiresUniqueChars),
        Description = $"A senha deve ter pelo menos {uniqueChars} caracteres únicos."
    };

    public override IdentityError RecoveryCodeRedemptionFailed() => new()
    {
        Code = nameof(RecoveryCodeRedemptionFailed),
        Description = "Falha ao resgatar o código de recuperação."
    };
}
