﻿using Neptuno2022EF.Datos.Interfaces;
using Neptuno2022EF.Datos;
using Neptuno2022EF.Entidades.Dtos.Proveedor;
using Neptuno2022EF.Servicios.Interfaces;
using System;
using System.Collections.Generic;
using Neptuno2022EF.Entidades.Entidades;

namespace Neptuno2022EF.Servicios.Servicios
{
    public class ServiciosProveedores:IServiciosProveedores
    {
        private readonly IRepositorioProveedores _repositorio;
        private readonly IUnitOfWork _unitOfWork;

        public ServiciosProveedores(IRepositorioProveedores repositorio, IUnitOfWork unitOfWork)
        {
            _repositorio = repositorio;
            _unitOfWork = unitOfWork;
        }

        public void Borrar(int id)
        {
            try
            {
                _repositorio.Borrar(id);
                _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool EstaRelacionado(Proveedor proveedor)
        {
            try
            {
                return _repositorio.EstaRelacionado(proveedor);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool Existe(Proveedor proveedor)
        {
            try
            {
                return _repositorio.Existe(proveedor);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ProveedorListDto> Filtrar(Func<Proveedor, bool> predicado)
        {
            try
            {
                return _repositorio.Filtrar(predicado);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Proveedor GetProveedorPorId(int id)
        {
            try
            {
                return _repositorio.GetProveedorPorId(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ProveedorListDto> GetProveedores()
        {
            try
            {
                return _repositorio.GetProveedores();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ProveedorListDto> GetProveedores(int paisId, int ciudadId)
        {
            try
            {
                return _repositorio.GetProveedores(paisId, ciudadId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Guardar(Proveedor proveedor)
        {
            try
            {
                if (proveedor.Id == 0)
                {
                    _repositorio.Agregar(proveedor);
                }
                else
                {
                    _repositorio.Editar(proveedor);
                }
                _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
