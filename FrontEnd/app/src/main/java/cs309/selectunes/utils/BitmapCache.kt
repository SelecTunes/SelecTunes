package cs309.selectunes.utils

import android.graphics.Bitmap
import android.util.LruCache

object BitmapCache {

    private const val cacheSize = 4 * 1024
    private val bitMapCache = object : LruCache<String, Bitmap>(cacheSize) {
        override fun sizeOf(key: String, value: Bitmap): Int {
            return value.byteCount
        }
    }

    fun loadOrStore(key: String, bitmap: Bitmap): Bitmap {
        if (bitMapCache[key] == null) {
            bitMapCache.trimToSize(cacheSize / 2)
            bitMapCache.put(key, bitmap)
            return bitmap
        }
        return bitMapCache[key]
    }

    fun store(key: String, bitmap: Bitmap) {
        bitMapCache.trimToSize(cacheSize / 2)
        bitMapCache.put(key, bitmap)
    }

    fun loadBitmap(key: String): Bitmap? {
        return bitMapCache[key]
    }

    fun clear() {
        bitMapCache.evictAll()
    }
}