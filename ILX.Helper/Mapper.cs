using AutoMapper;

namespace ILX.Helper
{
	public static class ClassAutoMapper
	{

		public static TDestination ConvertObject<TSource, TDestination>(this TSource source) where TSource : class
		{
			MapperConfiguration config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<TSource, TDestination>();
			});
			return config.CreateMapper().Map<TSource, TDestination>(source);
		}
	}
}
