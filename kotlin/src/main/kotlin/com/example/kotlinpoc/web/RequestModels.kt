package com.example.kotlinpoc.web

import com.fasterxml.jackson.annotation.JsonCreator
import com.fasterxml.jackson.annotation.JsonFormat
import java.math.BigDecimal

data class NewInvoice @JsonCreator constructor(
        val status: String,
        @JsonFormat(shape = JsonFormat.Shape.STRING)
        val amountDue: BigDecimal,
        val lineItems: List<NewLineItem>?
)

data class NewLineItem @JsonCreator constructor(
        val description: String?,
        val quantity: Double?,
        @JsonFormat(shape = JsonFormat.Shape.STRING)
        val unitAmount: BigDecimal?,
        @JsonFormat(shape = JsonFormat.Shape.STRING)
        val lineAmount: BigDecimal?
)

