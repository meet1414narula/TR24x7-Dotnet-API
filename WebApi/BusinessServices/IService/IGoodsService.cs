using System.Collections.Generic;
using BusinessEntities;

namespace BusinessServices
{
    /// <summary>
    /// Goods Service Contract
    /// </summary>
    public interface IGoodsService
    {
        GoodsResponseEntity GetGoods(int goodsId);
        List<GoodsResponseEntity> GetAllGoods();
        List<GoodsResponseEntity> GetGoodsByUser(UserEntity userEntity);
        long CreateGoods(GoodsRequestEntity goodsEntity);
        bool UpdateGoods(int goodsId,GoodsRequestEntity goodsEntity);
        bool DeleteGoods(int goodsId);
    }
}
