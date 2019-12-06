package com.example.kotlinpoc.persistence

import com.fasterxml.jackson.annotation.JsonBackReference
import org.hibernate.annotations.GenericGenerator
import java.math.BigDecimal
import java.util.*
import javax.persistence.*

@Entity
data class Invoice(
        @Id
        val id: UUID? = UUID.randomUUID(),
        var status: String,
        @Column(columnDefinition = "DECIMAL(7,3)")
        var amountDue: BigDecimal,
        @OneToMany(mappedBy = "invoice", cascade = [CascadeType.ALL], orphanRemoval = true)
        var lineItems: List<LineItem>?
)

@Entity
data class LineItem(
        @Id
        val id: UUID? = UUID.randomUUID(),
        var description: String?,
        var quantity: Double?,
        @Column(columnDefinition = "DECIMAL(7,3)")
        var unitAmount: BigDecimal?,
        @Column(columnDefinition = "DECIMAL(7,3)")
        var lineAmount: BigDecimal?,
        @ManyToOne(fetch = FetchType.LAZY)
        @JoinColumn(name = "invoice_id")
        @JsonBackReference
        var invoice: Invoice?
)


