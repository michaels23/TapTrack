package com.example.carappskeleton

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import java.time.OffsetDateTime

class MainActivity : AppCompatActivity() {
	override fun onCreate(savedInstanceState: Bundle?) {
		super.onCreate(savedInstanceState)
		setContentView(R.layout.activity_main)

		val btn = findViewById<Button>(R.id.sendButton)
		btn.setOnClickListener {
			val intent = Intent("com.companyname.taptrack.CAR_TAP_ACTION")
			intent.putExtra("label", "CarButton")
			intent.putExtra("timestamp", OffsetDateTime.now().toString())
			// Make it explicit to the MAUI app so Android reliably delivers it
			intent.setPackage("com.companyname.taptrack")
			sendBroadcast(intent)
		}
	}
}
