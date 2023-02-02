using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Balea.Abstractions;
using Balea.Provider.EntityFrameworkCore.DbContexts;
using Balea.Provider.EntityFrameworkCore.Entities;

using Microsoft.EntityFrameworkCore;

namespace Balea.Store.EntityFrameworkCore;

public class RoleStore : IRoleStore<RoleEntity>
{
    private readonly BaleaDbContext _context;
    private readonly IAppContextAccessor _contextAccessor;

    public RoleStore(
        BaleaDbContext context,
        IAppContextAccessor contextAccessor)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
    }

    public async Task<RoleEntity> FindByNameAsync(string roleName, CancellationToken cancellationToken)
    {
        return await _context.Roles
            .Where(x => x.Application.Name == _contextAccessor.AppContext.Name)
            .Where(x => x.Name == roleName)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<AccessResult> CreateAsync(RoleEntity role, CancellationToken cancellationToken)
    {
        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessResult.Success;
    }

    public async Task<AccessResult> UpdateAsync(RoleEntity role, CancellationToken cancellationToken)
    {
        _context.Roles.Update(role);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessResult.Success;
    }

    public async Task<AccessResult> DeleteAsync(RoleEntity role, CancellationToken cancellationToken)
    {
        _context.Roles.Remove(role);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessResult.Success;
    }

    public Task<string> GetNameAsync(RoleEntity role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.Name);
    }

    public Task<string> GetDescriptionAsync(RoleEntity role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.Description);
    }

    public Task<bool> IsEnabledAsync(RoleEntity role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.Enabled);
    }

    public async Task<AccessResult> AddMappingAsync(RoleEntity role, string mapping, CancellationToken cancellationToken)
    {
        var entity = new RoleMappingEntity
        {
            Role = role,
            Mapping = new MappingEntity(mapping)
        };

        await _context.RoleMappings.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessResult.Success;
    }

    public async Task<AccessResult> AddMappingsAsync(RoleEntity role, IList<string> mappings, CancellationToken cancellationToken)
    {
        foreach (var mapping in mappings)
        {
            var entity = new RoleMappingEntity
            {
                Role = role,
                Mapping = new MappingEntity(mapping)
            };

            await _context.RoleMappings.AddAsync(entity, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return AccessResult.Success;
    }

    public async Task<AccessResult> RemoveMappingAsync(RoleEntity role, string mapping, CancellationToken cancellationToken)
    {
        var binding = await _context.RoleMappings
            .Where(x => x.RoleId == role.Id)
            .Where(x => x.Mapping.Name == mapping)
            .FirstOrDefaultAsync(cancellationToken);

        _context.RoleMappings.Remove(binding);
        _context.Mappings.Remove(binding.Mapping);

        await _context.SaveChangesAsync(cancellationToken);

        return AccessResult.Success;
    }

    public async Task<AccessResult> RemoveMappingsAsync(RoleEntity role, IList<string> mappings, CancellationToken cancellationToken)
    {
        foreach (var mapping in mappings)
        {
            var binding = await _context.RoleMappings
                .Where(x => x.RoleId == role.Id)
                .Where(x => x.Mapping.Name == mapping)
                .FirstOrDefaultAsync(cancellationToken);

            _context.RoleMappings.Remove(binding);
            _context.Mappings.Remove(binding.Mapping);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return AccessResult.Success;
    }

    public async Task<IList<string>> GetMappingsAsync(RoleEntity role, CancellationToken cancellationToken)
    {
        return await _context.RoleMappings
            .Where(x => x.RoleId == role.Id)
            .Select(x => x.Mapping.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IList<RoleEntity>> FindByMappingAsync(string mapping, CancellationToken cancellationToken)
    {
        return await _context.RoleMappings
            .Where(x => x.Role.Application.Name == _contextAccessor.AppContext.Name)
            .Where(x => x.Mapping.Name == mapping)
            .Select(x => x.Role)
            .ToListAsync(cancellationToken);
    }

    public async Task<IList<RoleEntity>> FindByMappingsAsync(IList<string> mappings, CancellationToken cancellationToken)
    {
        return await _context.RoleMappings
            .Where(x => x.Role.Application.Name == _contextAccessor.AppContext.Name)
            .Where(x => mappings.Contains(x.Mapping.Name))
            .Select(x => x.Role)
            .ToListAsync(cancellationToken);
    }

    public async Task<AccessResult> AddPermissionAsync(RoleEntity role, string permission, CancellationToken cancellationToken)
    {
        var target = await _context.Permissions
            .Where(x => x.Application.Name == _contextAccessor.AppContext.Name)
            .Where(x => x.Name == permission)
            .FirstOrDefaultAsync(cancellationToken);

        var entity = new RolePermissionEntity
        {
            Role = role,
            Permission = target
        };

        await _context.RolePermissions.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessResult.Success;
    }

    public async Task<AccessResult> AddPermissionsAsync(RoleEntity role, IList<string> permissions, CancellationToken cancellationToken)
    {
        foreach (var permission in permissions)
        {
            var target = await _context.Permissions
                .Where(x => x.Application.Name == _contextAccessor.AppContext.Name)
                .Where(x => x.Name == permission)
                .FirstOrDefaultAsync(cancellationToken);

            var entity = new RolePermissionEntity
            {
                Role = role,
                Permission = target
            };

            await _context.RolePermissions.AddAsync(entity, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return AccessResult.Success;
    }

    public async Task<AccessResult> RemovePermissionAsync(RoleEntity role, string permission, CancellationToken cancellationToken)
    {
        var binding = await _context.RolePermissions
            .Where(x => x.RoleId == role.Id)
            .Where(x => x.Permission.Name == permission)
            .FirstOrDefaultAsync(cancellationToken);

        _context.RolePermissions.Remove(binding);

        await _context.SaveChangesAsync(cancellationToken);

        return AccessResult.Success;
    }

    public async Task<AccessResult> RemovePermissionsAsync(RoleEntity role, IList<string> permissions, CancellationToken cancellationToken)
    {
        foreach (var permission in permissions)
        {
            var binding = await _context.RolePermissions
                .Where(x => x.RoleId == role.Id)
                .Where(x => x.Permission.Name == permission)
                .FirstOrDefaultAsync(cancellationToken);

            _context.RolePermissions.Remove(binding);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return AccessResult.Success;
    }

    public async Task<IList<string>> GetPermissionsAsync(RoleEntity role, CancellationToken cancellationToken)
    {
        return await _context.RolePermissions
            .Where(x => x.RoleId == role.Id)
            .Select(x => x.Permission.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IList<RoleEntity>> FindByPermissionAsync(string permission, CancellationToken cancellationToken)
    {
        return await _context.RolePermissions
            .Where(x => x.Role.Application.Name == _contextAccessor.AppContext.Name)
            .Where(x => x.Permission.Name == permission)
            .Select(x => x.Role)
            .ToListAsync(cancellationToken);
    }

    public async Task<AccessResult> AddMemberAsync(RoleEntity role, string subject, CancellationToken cancellationToken)
    {
        var target = await _context.Subjects
            .Where(x => x.Sub == subject)
            .FirstOrDefaultAsync(cancellationToken);

        var entity = new RoleSubjectEntity
        {
            Role = role,
            Subject = target
        };

        await _context.RoleSubjects.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return AccessResult.Success;
    }

    public async Task<AccessResult> AddMembersAsync(RoleEntity role, IList<string> subjects, CancellationToken cancellationToken)
    {
        foreach (var subject in subjects)
        {
            var target = await _context.Subjects
                .Where(x => x.Sub == subject)
                .FirstOrDefaultAsync(cancellationToken);

            var entity = new RoleSubjectEntity
            {
                Role = role,
                Subject = target
            };

            await _context.RoleSubjects.AddAsync(entity, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return AccessResult.Success;
    }

    public async Task<AccessResult> RemoveMemberAsync(RoleEntity role, string subject, CancellationToken cancellationToken)
    {
        var binding = await _context.RoleSubjects
            .Where(x => x.RoleId == role.Id)
            .Where(x => x.Subject.Sub == subject)
            .FirstOrDefaultAsync(cancellationToken);

        _context.RoleSubjects.Remove(binding);

        await _context.SaveChangesAsync(cancellationToken);

        return AccessResult.Success;
    }

    public async Task<AccessResult> RemoveMembersAsync(RoleEntity role, IList<string> subjects, CancellationToken cancellationToken)
    {
        foreach (var subject in subjects)
        {
            var binding = await _context.RoleSubjects
                .Where(x => x.RoleId == role.Id)
                .Where(x => x.Subject.Sub == subject)
                .FirstOrDefaultAsync(cancellationToken);

            _context.RoleSubjects.Remove(binding);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return AccessResult.Success;
    }

    public async Task<IList<string>> GetMembersAsync(RoleEntity role, CancellationToken cancellationToken)
    {
        return await _context.RoleSubjects
            .Where(x => x.RoleId == role.Id)
            .Select(x => x.Subject.Sub)
            .ToListAsync(cancellationToken);
    }

    public async Task<IList<RoleEntity>> FindByMemberAsync(string subject, CancellationToken cancellationToken)
    {
        return await _context.RoleSubjects
            .Where(x => x.Role.Application.Name == _contextAccessor.AppContext.Name)
            .Where(x => x.Subject.Sub == subject)
            .Select(x => x.Role)
            .ToListAsync(cancellationToken);
    }
}