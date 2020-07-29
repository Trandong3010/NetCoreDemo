using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Infrastructure.Automapper
{
	public interface IAutoMapConverter<TSource, TDestination> where TSource : class where TDestination : class
	{
		TDestination ConvertObject(TSource srcObj);
		List<TDestination> ConvertObjectCollection(IEnumerable<TSource> srcObj);
	}

	public class AutoMapConverter<TSource, TDestination> : IAutoMapConverter<TSource, TDestination> where TSource : class where TDestination : class
	{
		private AutoMapper.IMapper mapper;

		public AutoMapConverter()
		{
			var config = new AutoMapper.MapperConfiguration(cfg =>
			{
				cfg.CreateMap<TSource, TDestination>();
			});
			mapper = config.CreateMapper();
		}

		public TDestination ConvertObject(TSource srcObj)
		{
			return mapper.Map<TSource, TDestination>(srcObj);
		}

		public List<TDestination> ConvertObjectCollection(IEnumerable<TSource> srcObj)
		{
			if (srcObj == null) return null;
			var destList = srcObj.Select(x => this.ConvertObject(x));
			return destList.ToList();
		}
	}
}
