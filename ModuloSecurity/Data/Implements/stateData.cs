﻿using Data.Interfaces;
using Entity.Context;
using Entity.DTO;
using Entity.Model.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implements
{
    public class stateData:IStateData
    {
        private readonly ApplicationDBContext context;
        protected readonly IConfiguration configuration;

        public stateData(ApplicationDBContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }
        public async Task Delete(int id)
        {
            var entity = await GetById(id);
            if (entity == null)
            {
                throw new Exception("Registro no encontrado");
            }

            context.States.Remove(entity);
            await context.SaveChangesAsync();
        }
        public async Task<IEnumerable<DataSelectDto>> GetAllSelect()
        {
            var sql = @"SELECT
                Id,
                CONCAT(Name, '-', name) AS TextoMostrar
                FROM
                state
                WHERE DeletedAt IS NULL AND State = 1
                ORDER BY Id ASC";
            return await context.QueryAsync<DataSelectDto>(sql);

        }
        public async Task<state> GetById(int id)
        {
            var sql = @"SELECT * FROM state WHERE Id = @Id ORDER BY Id ASC";
            return await this.context.QueryFirstOrDefaultAsync<state>(sql, new { Id = id });
        }
        public async Task<state> Save(state entity)
        {
            context.States.Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public async Task Update(state entity)
        {
            context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await context.SaveChangesAsync();
        }
        public async Task<state> GetByName(string name)
        {
            return await this.context.States.AsNoTracking().Where(item => item.name == name).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<state>> GetAll()
        {
            var sql = @"SELECT * FROM state ORDER BY Id ASC";
            return await this.context.QueryAsync<state>(sql);
        }

    }
}
