# Unity-ObjectPool

为了减少堆内存，减少实例化，添加对象缓存池。

1. 使用方式是：继承Recyclable<T>

	public class UIGoodsItem : RecyclableObj<UIGoodsItem>

2. 获取一个对象

	UIGoodsItem m_GoodItem = UITemp.CreateTempItem<UIGoodsItem>();

	或者 
	UIGoodsItem m_GoodItem  = UIGoodsItem.GenObj(prefab);

3. 回收

	m_GoodItem.__Recycle();

	在不用这些对象的时候 回收他它们。
