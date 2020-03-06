package cs309.selectunes.utils

import android.content.Context
import android.util.Log
import com.bumptech.glide.GlideBuilder
import com.bumptech.glide.module.AppGlideModule
import cs309.selectunes.BuildConfig

/**
 * @author Joshua Edwards
 */
class GlideModuleThing : AppGlideModule(){
    override fun applyOptions(context: Context, builder: GlideBuilder) {
        super.applyOptions(context, builder)
        if (BuildConfig.DEBUG) {
            builder.setLogLevel(Log.VERBOSE)
        }
    }
}