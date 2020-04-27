package cs309.selectunes.utils

import android.graphics.Bitmap
import android.util.LruCache

/**
 * A cache to store song images from Spotify.
 * @author Jack Goldsworth
 */
object BitmapCache {

    private const val cacheSize = 100
    private val bitMapCache = object : LruCache<String, Bitmap>(cacheSize) {
        override fun sizeOf(key: String, value: Bitmap): Int {
            return value.byteCount
        }
    }

    /**
     * Load a bitmap if it already exists, and store
     * it if it doesn't. Always return the bitmap.
     * @param key corresponding bitmap key.
     * @param bitmap bitmap to store.
     */
    fun loadOrStore(key: String, bitmap: Bitmap): Bitmap {
        if (bitMapCache[key] == null) {
            bitMapCache.put(key, bitmap)
            return bitmap
        }
        return bitMapCache[key]
    }

    /**
     * Store a bitmap in the cache.
     * @param key corresponding bitmap key.
     * @param bitmap bitmap to store.
     */
    fun store(key: String, bitmap: Bitmap) {
        bitMapCache.trimToSize(cacheSize / 2)
        bitMapCache.put(key, bitmap)
    }

    /**
     * load a bitmap from the cache.
     * @param key corresponding bitmap key.
     */
    fun loadBitmap(key: String): Bitmap? {
        return bitMapCache[key]
    }

    /**
     * Clear the cache.
     */
    fun clear() {
        bitMapCache.evictAll()
    }
}